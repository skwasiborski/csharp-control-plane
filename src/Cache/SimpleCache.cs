using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Envoy.Api.V2;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;

namespace Cache
{
    public interface ISnapshotCache : ICache
    {
        void SetSnapshot(string node, Snapshot snapshot);
    }

    public class SimpleCache : ISnapshotCache
    {
        private readonly ConcurrentDictionary<string, NodeInfo> _nodeInfos =
            new ConcurrentDictionary<string, NodeInfo>();

        public void SetSnapshot(string node, Snapshot snapshot)
        {
            var info = _nodeInfos.GetOrAdd(node, id => new NodeInfo());

            IEnumerable<(LastKnowRequest lastKnowRequest, DiscoveryResponse response)> requestsToResolve;

            lock (info.Lock)
            {
                info.Snapshot = snapshot;

                requestsToResolve = info.LastKnownRequests
                    .Where(kv => kv.Value.Request.VersionInfo != snapshot.GetVersion(kv.Value.Request.TypeUrl))
                    .Select(kv => (
                        kv.Value,
                        CreateResponse(
                            kv.Value.Request,
                            snapshot.GetResources(kv.Value.Request.TypeUrl),
                            snapshot.GetVersion(kv.Value.Request.TypeUrl))))
                    .ToArray(); // force the evaluation in lock
            }

            foreach (var request in requestsToResolve)
            {
                request.lastKnowRequest.Resolve(request.response);
            }
        }

        public ValueTask<DiscoveryResponse> GetResponseForStream(DiscoveryRequest request)
        {
            LastKnowRequest lastKnowRequest;
            bool resolveImmediatly = false;

            var info = _nodeInfos.GetOrAdd(request.Node.Id, id => new NodeInfo());

            lock (info.Lock)
            {
                var oldResponseExist = info.LastKnownRequests.TryGetValue(request.TypeUrl, out var previousResponse);

                if (info.Snapshot != null &&
                    request.VersionInfo != info.Snapshot.GetVersion(request.TypeUrl))
                {
                    // the version envoy knows is older then snapshot respond immediatly.
                    resolveImmediatly = true;
                }
                else if (oldResponseExist &&
                         previousResponse.Request.ResourceNames != request.ResourceNames) // TODO: proper comprison
                {
                    // envoy requested new resources respond immediatly.
                    resolveImmediatly = true;
                }

                if (oldResponseExist)
                {
                    // the old response should not be used.
                    previousResponse.Fault();
                }

                lastKnowRequest = new LastKnowRequest(request);
                info.LastKnownRequests[request.TypeUrl] = lastKnowRequest;
            }

            if (resolveImmediatly &&
                info.Snapshot != null
            ) // TODO: check if we have all requested resources (if not leave the response pending)
            {
                // We alredy have all data for this request and there is new data (as compared to last known requast)
                var resources = info.Snapshot.GetResources(request.TypeUrl);

                lastKnowRequest.Resolve(
                    CreateResponse(request, resources, info.Snapshot.GetVersion(request.TypeUrl)));
            }

            return new ValueTask<DiscoveryResponse>(lastKnowRequest.Response);
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

        private class LastKnowRequest
        {
            private readonly TaskCompletionSource<DiscoveryResponse> _tcs;
            private readonly DiscoveryRequest _request;

            public Task<DiscoveryResponse> Response => _tcs.Task;

            public DiscoveryRequest Request => _request;

            public LastKnowRequest(DiscoveryRequest request)
            {
                _request = request;
                _tcs = new TaskCompletionSource<DiscoveryResponse>(TaskCreationOptions.RunContinuationsAsynchronously);
            }

            public void Resolve(DiscoveryResponse response)
            {
                // using try here because there can be ace codition when setting new snapshot and processing new request rom envoy at the same time
                // regardless who wins the appropriate data will be send.
                _tcs.TrySetResult(response);
            }

            public void Fault()
            {
                _tcs.TrySetException(new InvalidOperationException("This response was sperceeded by new request"));
            }
        }

        private class NodeInfo
        {
            private readonly object _lock = new object();

            public Snapshot Snapshot { get; set; }
            public Dictionary<string, LastKnowRequest> LastKnownRequests { get; }

            public object Lock => _lock;

            public NodeInfo()
            {
                Snapshot = null;
                LastKnownRequests = new Dictionary<string, LastKnowRequest>();
            }
        }
    }

    public class Snapshot
    {
        public Resources Endpoints { get; }
        public Resources Clusters { get; }
        public Resources Routes { get; }
        public Resources Listiners { get; }

        public string GetVersion(string type)
        {
            return GetByType(type, e => e.Version);
        }

        public ImmutableDictionary<string, IMessage> GetResources(string type)
        {
            return GetByType(type, e => e.Items);
        }

        private T GetByType<T>(
            string type,
            Func<Resources, T> selector)
        {
            switch (type)
            {
                case TypeStrings.EndpointType:
                    return selector(Endpoints);
                case TypeStrings.ClusterType:
                    return selector(Clusters);
                case TypeStrings.RouteType:
                    return selector(Routes);
                case TypeStrings.ListenerType:
                    return selector(Listiners);
                default:
                    throw new ArgumentOutOfRangeException(type);
            }
        }
    }

    public class Resources
    {
        public string Version { get; }
        public ImmutableDictionary<string, IMessage> Items { get; }

        public Resources(string version, ImmutableDictionary<string, IMessage> items)
        {
            Items = items;
            Version = version;
        }
    }

    public static class TypeStrings
    {
        public const string TypePrefix = "type.googleapis.com/envoy.api.v2.";
        public const string EndpointType = TypePrefix + "ClusterLoadAssignment";
        public const string ClusterType = TypePrefix + "Cluster";
        public const string RouteType = TypePrefix + "RouteConfiguration";
        public const string ListenerType = TypePrefix + "Listener";
    }
}