using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Envoy.Api.V2;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;

namespace Envoy.ControlPlane.Cache
{
    public interface ISnapshotCache : ICache
    {
        void SetSnapshot(string node, Snapshot snapshot);
    }

    public class SimpleCache : ISnapshotCache
    {
        private readonly ConcurrentDictionary<string, NodeInfo> _nodeInfos =
            new ConcurrentDictionary<string, NodeInfo>();

        private int _pendingResponseId = 0;
        
        public void SetSnapshot(string node, Snapshot snapshot)
        {
            var info = _nodeInfos.GetOrAdd(node, id => new NodeInfo());

            IEnumerable<(KeyValuePair<int, PendingResponse> pendingResponse, DiscoveryResponse response)> requestsToResolve;

            lock (info.Lock)
            {
                info.Snapshot = snapshot;

                requestsToResolve = info.PendingResponses
                    .Where(kv => kv.Value.Request.VersionInfo != snapshot.GetVersion(kv.Value.Request.TypeUrl))
                    .Select(kv => (
                        kv,
                        CreateResponse(
                            kv.Value.Request,
                            snapshot.GetResources(kv.Value.Request.TypeUrl),
                            snapshot.GetVersion(kv.Value.Request.TypeUrl))))
                    .ToArray(); // force the evaluation in lock

                foreach (var r in requestsToResolve)
                {
                    info.PendingResponses.Remove(r.pendingResponse.Key);
                }
            }

            foreach (var request in requestsToResolve)
            {
                request.pendingResponse.Value.Resolve(request.response);
            }
        }

        public Watch CreateWatch(DiscoveryRequest request)
        {

            var info = _nodeInfos.GetOrAdd(request.Node.Id, id => new NodeInfo());

            lock (info.Lock)
            {
                if (info.Snapshot == null || request.VersionInfo == info.Snapshot.GetVersion(request.TypeUrl))
                {
                    var watchId = Interlocked.Increment(ref _pendingResponseId);
                    var currentRequest = new PendingResponse(request);
                    info.PendingResponses[watchId] = currentRequest;
                    
                    return new Watch(
                        currentRequest.Response, 
                        () => CancelWatch(request.Node.Id, watchId));
                }
            }
            
            var resources = info.Snapshot.GetResources(request.TypeUrl);
            return new Watch (
                Task.FromResult(CreateResponse(request, resources, info.Snapshot.GetVersion(request.TypeUrl))), 
                Watch.NoOp);
        }

        public ValueTask<DiscoveryResponse> GetResponseForFetch(DiscoveryRequest request)
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
            private readonly DiscoveryRequest _request;

            public Task<DiscoveryResponse> Response => _tcs.Task;

            public DiscoveryRequest Request => _request;

            public PendingResponse(DiscoveryRequest request)
            {
                _request = request;
                _tcs = new TaskCompletionSource<DiscoveryResponse>(TaskCreationOptions.RunContinuationsAsynchronously);
            }

            public void Resolve(DiscoveryResponse response)
            {
                // using try here because there can be race codition when setting new snapshot and processing new request rom envoy at the same time
                // regardless who wins the appropriate data will be send.
                _tcs.TrySetResult(response);
            }
        }

        private class NodeInfo
        {
            private readonly object _lock = new object();

            public Snapshot Snapshot { get; set; }
            public Dictionary<int, PendingResponse> PendingResponses { get; }

            public object Lock => _lock;

            public NodeInfo()
            {
                Snapshot = null;
                PendingResponses = new Dictionary<int, PendingResponse>();
            }
        }
    }
}