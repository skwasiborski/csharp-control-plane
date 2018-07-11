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

        public ClusterDiscoveryService.ClusterDiscoveryServiceBase ClusterService => _clusterService;
        public EndpointDiscoveryService.EndpointDiscoveryServiceBase EndpointService => _endpointService;
        public ListenerDiscoveryService.ListenerDiscoveryServiceBase ListenerService => _listenerService;
        public RouteDiscoveryService.RouteDiscoveryServiceBase RouteService => _routeService;
        public AggregatedDiscoveryService.AggregatedDiscoveryServiceBase AggregatedService => _aggregatedService;
        
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

        private async Task HandleStream(IAsyncStreamReader<DiscoveryRequest> requestStream,
            IServerStreamWriter<DiscoveryResponse> responseStream,
            ServerCallContext context,
            string defaultTypeUrl)
        {
            var watches = new Watches
            {
                Endpoints = {Watch = Watch.Empty},
                Clusters = {Watch = Watch.Empty},
                Listeners = {Watch = Watch.Empty},
                Routes = {Watch = Watch.Empty}
            };

            try
            {
                var streamNonce = 0;

                Task Send(DiscoveryResponse response, WatchAndNounce watchAndNounce)
                {
                    response.Nonce = (++streamNonce).ToString();
                    watchAndNounce.Nonce = streamNonce.ToString();

                    // the value is consumed we do not want to get it again from this watch. 
                    watchAndNounce.Watch.Cancel();
                    watchAndNounce.Watch = Watch.Empty; 

                    return responseStream.WriteAsync(response);
                }

                var requestTask = requestStream.MoveNext(_cancellationToken);

                while (!_cancellationToken.IsCancellationRequested &&
                       !context.CancellationToken.IsCancellationRequested)
                {
                    var resolved = await Task.WhenAny(
                        requestTask,
                        watches.Clusters.Watch.Response,
                        watches.Endpoints.Watch.Response,
                        watches.Listeners.Watch.Response,
                        watches.Routes.Watch.Response);

                    switch (resolved)
                    {
                        case Task<bool> r when ReferenceEquals(r, requestTask):
                            // New request from Envoy
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
                                         (watches.Clusters.Nonce == null ||
                                          request.ResponseNonce == watches.Clusters.Nonce):
                                    watches.Clusters.Watch.Cancel();
                                    watches.Clusters.Watch = _cache.CreateWatch(request);
                                    break;
                                case var typeUrl
                                    when typeUrl == TypeStrings.EndpointType &&
                                         (watches.Endpoints.Nonce == null ||
                                          request.ResponseNonce == watches.Endpoints.Nonce):
                                    watches.Endpoints.Watch.Cancel();
                                    watches.Endpoints.Watch = _cache.CreateWatch(request);
                                    break;
                                case var typeUrl
                                    when typeUrl == TypeStrings.ListenerType &&
                                         (watches.Listeners.Nonce == null ||
                                          request.ResponseNonce == watches.Listeners.Nonce):
                                    watches.Listeners.Watch.Cancel();
                                    watches.Listeners.Watch = _cache.CreateWatch(request);
                                    break;
                                case var typeUrl
                                    when typeUrl == TypeStrings.RouteType &&
                                         (watches.Clusters.Nonce == null || 
                                          request.ResponseNonce == watches.Routes.Nonce):
                                    watches.Routes.Watch.Cancel();
                                    watches.Routes.Watch = _cache.CreateWatch(request);
                                    break;
                            }

                            requestTask = requestStream.MoveNext(_cancellationToken);
                            break;
                        // Check if any watch was resolved. If yes send he update.
                        case Task<DiscoveryResponse> response
                            when ReferenceEquals(response, watches.Clusters.Watch.Response):
                            await Send(response.Result, watches.Clusters);
                            break;
                        case Task<DiscoveryResponse> response
                            when ReferenceEquals(response, watches.Endpoints.Watch.Response):
                            await Send(response.Result, watches.Endpoints);
                            break;
                        case Task<DiscoveryResponse> response
                            when ReferenceEquals(response, watches.Listeners.Watch.Response):
                            await Send(response.Result, watches.Listeners);
                            break;
                        case Task<DiscoveryResponse> response
                            when ReferenceEquals(response, watches.Routes.Watch.Response):
                            await Send(response.Result, watches.Routes);
                            break;
                    }
                }
            }
            finally
            {
                // cleanup the cahce before exit
                watches.Clusters.Watch.Cancel();
                watches.Endpoints.Watch.Cancel();
                watches.Listeners.Watch.Cancel();
                watches.Routes.Watch.Cancel();
            }
        }

        private struct Watches
        {
            public WatchAndNounce Endpoints;
            public WatchAndNounce Clusters;
            public WatchAndNounce Listeners;
            public WatchAndNounce Routes;
        }

        private struct WatchAndNounce
        {
            public Watch Watch;
            public string Nonce;
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