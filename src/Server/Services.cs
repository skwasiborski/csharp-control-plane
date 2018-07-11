using System;
using System.Threading;
using System.Threading.Tasks;
using Envoy.Api.V2;
using Envoy.ControlPlane.Cache;
using Envoy.Service.Discovery.V2;
using Grpc.Core;

namespace Envoy.ControlPlane.Server
{
    public class Services
    {
        private readonly ICache _cache;
        private readonly CancellationToken _cancellationToken;
        private readonly ClusterDiscoveryServiceImpl _clusterService;
        private readonly EndpointDiscoveryServiceImpl _endpointService;
        private readonly ListenerDiscoveryServiceImpl _listenerService;
        private readonly RouteDiscoveryServiceImpl _routeService;
        private readonly AggregatedDiscoveryServiceImpl _aggregatedService;

        public Services(ICache cache, CancellationToken cancellationToken)
        {
            _cache = cache;
            _cancellationToken = cancellationToken;
            _clusterService = new ClusterDiscoveryServiceImpl(this);
            _endpointService = new EndpointDiscoveryServiceImpl(this);
            _listenerService = new ListenerDiscoveryServiceImpl(this);
            _routeService = new RouteDiscoveryServiceImpl(this);
            _aggregatedService = new AggregatedDiscoveryServiceImpl(this);
        }

        public ClusterDiscoveryService.ClusterDiscoveryServiceBase ClusterService => _clusterService;
        public EndpointDiscoveryService.EndpointDiscoveryServiceBase EndpointService => _endpointService;
        public ListenerDiscoveryService.ListenerDiscoveryServiceBase ListenerService => _listenerService;
        public RouteDiscoveryService.RouteDiscoveryServiceBase RouteService => _routeService;
        public AggregatedDiscoveryService.AggregatedDiscoveryServiceBase AggregatedService => _aggregatedService;

        private async Task HandleStream(IAsyncStreamReader<DiscoveryRequest> requestStream,
            IServerStreamWriter<DiscoveryResponse> responseStream,
            ServerCallContext context,
            string defaultTypeUrl)
        {
            var watches = new Watches
            {
                Endpoints = Watch.Empty,
                Clusters = Watch.Empty,
                Listeners = Watch.Empty,
                Routes = Watch.Empty
            };

            try
            {
                var streamNonce = 0;

                Task Send(DiscoveryResponse response, ref int nonce)
                {
                    response.Nonce = Interlocked.Increment(ref nonce).ToString();
                    return responseStream.WriteAsync(response);
                }

                var requestTask = requestStream.MoveNext(_cancellationToken);

                while (!_cancellationToken.IsCancellationRequested &&
                       !context.CancellationToken.IsCancellationRequested)
                {
                    var resolved = await Task.WhenAny(
                        requestTask,
                        watches.Clusters.Response,
                        watches.Endpoints.Response,
                        watches.Listeners.Response,
                        watches.Routes.Response);

                    switch (resolved)
                    {
                        case Task<bool> r when ReferenceEquals(r, requestTask):
                            if (!r.Result)
                            {
                                // MoveNext failed. The strem was finalized by Envoy or is broken
                                throw new OperationCanceledException();
                            }

                            var request = requestStream.Current;
                            if (defaultTypeUrl == TypeStrings.Any && string.IsNullOrEmpty(request.TypeUrl))
                            {
                                throw new InvalidOperationException("type URL is required for ADS");
                            }

                            if (string.IsNullOrEmpty(request.TypeUrl))
                            {
                                request.TypeUrl = defaultTypeUrl;
                            }

                            switch (request.TypeUrl)
                            {
                                case var typeUrl
                                    when typeUrl == TypeStrings.ClusterType &&
                                         (watches.ClusterNonce == null ||
                                          request.ResponseNonce == watches.ClusterNonce):
                                    watches.Clusters.Cancel();
                                    watches.Clusters = _cache.CreateWatch(request);
                                    break;
                                case var typeUrl
                                    when typeUrl == TypeStrings.EndpointType &&
                                         (watches.EndpointNonce == null ||
                                          request.ResponseNonce == watches.EndpointNonce):
                                    watches.Endpoints.Cancel();
                                    watches.Endpoints = _cache.CreateWatch(request);
                                    break;
                                case var typeUrl
                                    when typeUrl == TypeStrings.ListenerType &&
                                         (watches.ListenerNonce == null ||
                                          request.ResponseNonce == watches.ListenerNonce):
                                    watches.Listeners.Cancel();
                                    watches.Listeners = _cache.CreateWatch(request);
                                    break;
                                case var typeUrl
                                    when typeUrl == TypeStrings.RouteType &&
                                         (watches.ClusterNonce == null || request.ResponseNonce == watches.RouteNonce):
                                    watches.Routes.Cancel();
                                    watches.Routes = _cache.CreateWatch(request);
                                    break;
                            }

                            requestTask = requestStream.MoveNext(_cancellationToken);
                            break;
                        case Task<DiscoveryResponse> response
                            when ReferenceEquals(response, watches.Clusters.Response):
                            await Send(response.Result, ref streamNonce);
                            watches.ClusterNonce = streamNonce.ToString();
                            watches.Clusters.Cancel();
                            watches.Clusters =
                                Watch.Empty; // the value is consumed we do not want to get it again from this watch
                            break;
                        case Task<DiscoveryResponse> response
                            when ReferenceEquals(response, watches.Endpoints.Response):
                            await Send(response.Result, ref streamNonce);
                            watches.EndpointNonce = streamNonce.ToString();
                            watches.Endpoints.Cancel();
                            watches.Endpoints =
                                Watch.Empty; // the value is consumed we do not want to get it again from this watch
                            break;
                        case Task<DiscoveryResponse> response
                            when ReferenceEquals(response, watches.Listeners.Response):
                            await Send(response.Result, ref streamNonce);
                            watches.ListenerNonce = streamNonce.ToString();
                            watches.Listeners.Cancel();
                            watches.Listeners =
                                Watch.Empty; // the value is consumed we do not want to get it again from this watch
                            break;
                        case Task<DiscoveryResponse> response
                            when ReferenceEquals(response, watches.Routes.Response):
                            await Send(response.Result, ref streamNonce);
                            watches.RouteNonce = streamNonce.ToString();
                            watches.Routes.Cancel();
                            watches.Routes =
                                Watch.Empty; // the value is consumed we do not want to get it again from this watch
                            break;
                    }
                }
            }
            finally
            {
                // cleanup the cahce before exit
                watches.Clusters.Cancel();
                watches.Endpoints.Cancel();
                watches.Listeners.Cancel();
                watches.Routes.Cancel();
            }
        }

        private struct Watches
        {
            public Watch Endpoints { get; set; }
            public Watch Clusters { get; set; }
            public Watch Listeners { get; set; }
            public Watch Routes { get; set; }

            public string EndpointNonce { get; set; }
            public string ClusterNonce { get; set; }
            public string RouteNonce { get; set; }
            public string ListenerNonce { get; set; }
        }

        private class ClusterDiscoveryServiceImpl : ClusterDiscoveryService.ClusterDiscoveryServiceBase
        {
            private readonly Services _services;

            public ClusterDiscoveryServiceImpl(Services services)
            {
                _services = services;
            }

            public override Task StreamClusters(IAsyncStreamReader<DiscoveryRequest> requestStream,
                IServerStreamWriter<DiscoveryResponse> responseStream, ServerCallContext context)
            {
                return _services.HandleStream(requestStream, responseStream, context, TypeStrings.ClusterType);
            }
        }

        private class EndpointDiscoveryServiceImpl : EndpointDiscoveryService.EndpointDiscoveryServiceBase
        {
            private readonly Services _services;

            public EndpointDiscoveryServiceImpl(Services services)
            {
                _services = services;
            }

            public override Task StreamEndpoints(IAsyncStreamReader<DiscoveryRequest> requestStream,
                IServerStreamWriter<DiscoveryResponse> responseStream, ServerCallContext context)
            {
                return _services.HandleStream(requestStream, responseStream, context, TypeStrings.EndpointType);
            }
        }

        private class ListenerDiscoveryServiceImpl : ListenerDiscoveryService.ListenerDiscoveryServiceBase
        {
            private readonly Services _services;

            public ListenerDiscoveryServiceImpl(Services services)
            {
                _services = services;
            }

            public override Task StreamListeners(IAsyncStreamReader<DiscoveryRequest> requestStream,
                IServerStreamWriter<DiscoveryResponse> responseStream, ServerCallContext context)
            {
                return _services.HandleStream(requestStream, responseStream, context, TypeStrings.ListenerType);
            }
        }

        private class RouteDiscoveryServiceImpl : RouteDiscoveryService.RouteDiscoveryServiceBase
        {
            private readonly Services _services;

            public RouteDiscoveryServiceImpl(Services services)
            {
                _services = services;
            }

            public override Task StreamRoutes(IAsyncStreamReader<DiscoveryRequest> requestStream,
                IServerStreamWriter<DiscoveryResponse> responseStream, ServerCallContext context)
            {
                return _services.HandleStream(requestStream, responseStream, context, TypeStrings.RouteType);
            }
        }

        private class AggregatedDiscoveryServiceImpl : AggregatedDiscoveryService.AggregatedDiscoveryServiceBase
        {
            private readonly Services _services;

            public AggregatedDiscoveryServiceImpl(Services services)
            {
                _services = services;
            }

            public override Task StreamAggregatedResources(IAsyncStreamReader<DiscoveryRequest> requestStream,
                IServerStreamWriter<DiscoveryResponse> responseStream,
                ServerCallContext context)
            {
                return _services.HandleStream(requestStream, responseStream, context, TypeStrings.Any);
            }
        }
    }
}