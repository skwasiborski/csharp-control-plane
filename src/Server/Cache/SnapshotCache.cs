using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Envoy.Api.V2;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core.Logging;

namespace Envoy.ControlPlane.Server.Cache
{
    public interface ISnapshotCache : ICache
    {
        void SetSnapshot(string node, Snapshot snapshot);
    }

    /// <summary>
    /// Description taken from go-control-plane (https://github.com/envoyproxy/go-control-plane) snapshot cache.
    /// All the following considirations apply to this cache.
    /// 
    /// SnapshotCache is a snapshot-based cache that maintains a single versioned
    /// snapshot of responses per node. SnapshotCache consistently replies with the
    /// latest snapshot. For the protocol to work correctly in ADS mode, EDS/RDS
    /// requests are responded only when all resources in the snapshot xDS response
    /// are named as part of the request. It is expected that the CDS response names
    /// all EDS clusters, and the LDS response names all RDS routes in a snapshot,
    /// to ensure that Envoy makes the request for all EDS clusters or RDS routes
    /// eventually.
    ///
    /// SnapshotCache can operate as a REST or regular xDS backend. The snapshot
    /// can be partial, e.g. only include RDS or EDS resources.
    /// </summary>
    public class SnapshotCache : ISnapshotCache
    {
        private readonly bool _adsMode;
        private readonly ILogger _logger;

        private readonly ConcurrentDictionary<string, NodeInfo> _nodeInfos =
            new ConcurrentDictionary<string, NodeInfo>();

        private int _watchId = 0;

        public SnapshotCache(bool adsMode, ILogger logger)
        {
            _adsMode = adsMode;
            _logger = logger.ForType<SnapshotCache>();
        }
        
        public void SetSnapshot(string node, Snapshot snapshot)
        {
            var info = _nodeInfos.GetOrAdd(node, id => new NodeInfo());

            IEnumerable<(KeyValuePair<int, PendingResponse> pendingResponse, DiscoveryResponse response)> requestsToResolve;

            _logger.InfoF($"Setting new snapshot with versions cluster: {snapshot.Clusters.Version}, endpoints: {snapshot.Endpoints.Version}, listeners: {snapshot.Listiners.Version}, routes: {snapshot.Routes.Version}");
            
            lock (info.Lock)
            {
                info.Snapshot = snapshot;

                requestsToResolve = info.PendingResponses
                    .Where(kv => kv.Value.Request.VersionInfo != snapshot.GetVersion(kv.Value.Request.TypeUrl))
                    .Select(kv => (
                        kv,
                        CreateStreamResponse(
                            kv.Value.Request,
                            snapshot.GetResources(kv.Value.Request.TypeUrl),
                            snapshot.GetVersion(kv.Value.Request.TypeUrl))))
                    .ToArray();

                foreach (var r in requestsToResolve)
                {
                    info.PendingResponses.Remove(r.pendingResponse.Key);
                }
            }

            foreach (var request in requestsToResolve)
            {
                if (request.response == null)
                {
                    // Incomplete request in ADS mode. Do not respond.
                    continue;
                }

                _logger.DebugF($"-> Responding from new snapshot TypeUrl: {request.response.TypeUrl}, Version: {request.response.VersionInfo}");
                request.pendingResponse.Value.Resolve(request.response);
            }
        }

        public Watch CreateWatch(DiscoveryRequest request)
        {
            var info = _nodeInfos.GetOrAdd(request.Node.Id, id => new NodeInfo());
            ImmutableDictionary<string, IMessage> resources;
            
            lock (info.Lock)
            {
                if (info.Snapshot == null ||
                    request.VersionInfo == info.Snapshot.GetVersion(request.TypeUrl) ||
                    request.ErrorDetail != null)
                {
                    var watchId = Interlocked.Increment(ref _watchId);
                    var pendingResponse = new PendingResponse(request);
                    info.PendingResponses[watchId] = pendingResponse;
                
                    _logger.DebugF($"Returning pending response TypeUrl: {request.TypeUrl}, Version: {request.VersionInfo}, Nonce: {request.ResponseNonce}");

                    return new Watch(
                        pendingResponse.Response, 
                        () => CancelWatch(pendingResponse.Request.Node.Id, watchId));
                }

                resources = info.Snapshot.GetResources(request.TypeUrl);
            }
            
            var response = CreateStreamResponse(request, resources, info.Snapshot.GetVersion(request.TypeUrl));

            if (response == null)
            {
                // Incomplete request in ADS mode. Do not respond.
                return Watch.Empty;
            }

            _logger.DebugF($"-> Returning immediate response TypeUrl: {response.TypeUrl}, Version: {response.VersionInfo}");

            return new Watch(
                Task.FromResult(response),
                Watch.NoOp);
        }

        public ValueTask<DiscoveryResponse> Fetch(DiscoveryRequest request)
        {
            if (!_nodeInfos.TryGetValue(request.Node.Id, out var info) || info.Snapshot == null)
            {
                return new ValueTask<DiscoveryResponse>((DiscoveryResponse) null);
            }

            var version = info.Snapshot.GetVersion(request.TypeUrl);
            if (request.VersionInfo == version)
            {
                return new ValueTask<DiscoveryResponse>((DiscoveryResponse) null);
            }

            var resources = info.Snapshot.GetResources(request.TypeUrl);
            return new ValueTask<DiscoveryResponse>(CreateResponse(request, resources, version));
        }

        private void CancelWatch(string nodeId, int watchId)
        {
            var info = _nodeInfos.GetOrAdd(nodeId, id => new NodeInfo());
            lock (info.Lock)
            {
                info.PendingResponses.Remove(watchId);
            }
        }

        private DiscoveryResponse CreateStreamResponse( 
            DiscoveryRequest request,
            ImmutableDictionary<string, IMessage> resources,
            string version)
        {
            if (request.ResourceNames.Count > 0 && 
                _adsMode &&
                request.ResourceNames.Intersect(resources.Keys).Count() < resources.Count)
            {
                // for ADS, the request names must match the snapshot names
                // if they do not, then the watch is never responded, and it is expected that envoy makes another request
                
                _logger.DebugF($"Skipping response due to missinge resources in ads mode for request TypeUrl: {request.TypeUrl}, Version: {request.VersionInfo}, Nonce: {request.ResponseNonce}");
                return null;
            }

            return CreateResponse(request, resources, version);
        }
        
        private DiscoveryResponse CreateResponse(
            DiscoveryRequest request,
            ImmutableDictionary<string, IMessage> resources,
            string version)
        {
            IEnumerable<IMessage> resourcesToSend;

            if (request.ResourceNames.Count > 0)
            {
                var requestedNames = request.ResourceNames.ToHashSet();
                resourcesToSend = resources.Where(kv => requestedNames.Contains(kv.Key)).Select(kv => kv.Value);
            }
            else
            {
                resourcesToSend = resources.Values.ToList();
            }

            var response = new DiscoveryResponse
            {
                VersionInfo = version,
                TypeUrl = request.TypeUrl,
                Resources = {resourcesToSend.Select(Any.Pack)}
            };

            return response;
        }

        private class PendingResponse
        {
            private readonly TaskCompletionSource<DiscoveryResponse> _tcs;

            public Task<DiscoveryResponse> Response => _tcs.Task;
            public DiscoveryRequest Request { get; }

            public PendingResponse(DiscoveryRequest request)
            {
                Request = request;
                _tcs = new TaskCompletionSource<DiscoveryResponse>(TaskCreationOptions.RunContinuationsAsynchronously);
            }

            public void Resolve(DiscoveryResponse response)
            {
                // using try here because there can be race codition when setting new snapshot
                // and processing new request from envoy at the same time
                // regardless who wins the appropriate data will be send.
                _tcs.TrySetResult(response);
            }
        }

        private class NodeInfo
        {
            public Snapshot Snapshot { get; set; }
            public Dictionary<int, PendingResponse> PendingResponses { get; }

            public object Lock { get; } = new object();

            public NodeInfo()
            {
                Snapshot = null;
                PendingResponses = new Dictionary<int, PendingResponse>();
            }
        }
    }
}