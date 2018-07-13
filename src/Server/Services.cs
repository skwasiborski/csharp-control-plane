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
            var streamId = Guid.NewGuid();
            Console.WriteLine($"New Stream started {streamId}");

            var endpoints = new WatchAndNounce();
            var clusters = new WatchAndNounce();
            var listeners = new WatchAndNounce();
            var routes = new WatchAndNounce();

            try
            {
                var streamNonce = 0;

                Task Send(DiscoveryResponse response, WatchAndNounce watchAndNounce)
                {
                    response.Nonce = (++streamNonce).ToString();
                    watchAndNounce.Nonce = streamNonce.ToString();

                    Console.WriteLine(
                        $"-> New response on stream {streamId}, typeUrl {response.TypeUrl}, version {response.VersionInfo}, nonce: {response.Nonce}");

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
                        clusters.Watch.Response,
                        endpoints.Watch.Response,
                        listeners.Watch.Response,
                        routes.Watch.Response);

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

                            Console.WriteLine(
                                $"<- New request on stream {streamId}, typeUrl {request.TypeUrl}, version {request.VersionInfo}, nonce: {request.ResponseNonce}");

                            switch (request.TypeUrl)
                            {
                                case var typeUrl
                                    when typeUrl == TypeStrings.ClusterType &&
                                         (clusters.Nonce == null ||
                                          request.ResponseNonce == clusters.Nonce):
                                    clusters.Watch.Cancel();
                                    clusters.Watch = _cache.CreateWatch(request);
                                    break;
                                case var typeUrl
                                    when typeUrl == TypeStrings.EndpointType &&
                                         (endpoints.Nonce == null ||
                                          request.ResponseNonce == endpoints.Nonce):
                                    endpoints.Watch.Cancel();
                                    endpoints.Watch = _cache.CreateWatch(request);
                                    break;
                                case var typeUrl
                                    when typeUrl == TypeStrings.ListenerType &&
                                         (listeners.Nonce == null ||
                                          request.ResponseNonce == listeners.Nonce):
                                    listeners.Watch.Cancel();
                                    listeners.Watch = _cache.CreateWatch(request);
                                    break;
                                case var typeUrl
                                    when typeUrl == TypeStrings.RouteType &&
                                         (routes.Nonce == null ||
                                          request.ResponseNonce == routes.Nonce):
                                    routes.Watch.Cancel();
                                    routes.Watch = _cache.CreateWatch(request);
                                    break;
                            }

                            requestTask = requestStream.MoveNext(_cancellationToken);
                            break;
                        // Check if any watch was resolved. If yes send he update.
                        case Task<DiscoveryResponse> response
                            when ReferenceEquals(response, clusters.Watch.Response):
                            await Send(response.Result, clusters);
                            break;
                        case Task<DiscoveryResponse> response
                            when ReferenceEquals(response, endpoints.Watch.Response):
                            await Send(response.Result, endpoints);
                            break;
                        case Task<DiscoveryResponse> response
                            when ReferenceEquals(response, listeners.Watch.Response):
                            await Send(response.Result, listeners);
                            break;
                        case Task<DiscoveryResponse> response
                            when ReferenceEquals(response, routes.Watch.Response):
                            await Send(response.Result, routes);
                            break;
                    }
                }
            }
            finally
            {
                // cleanup the cahce before exit
                clusters.Watch.Cancel();
                endpoints.Watch.Cancel();
                listeners.Watch.Cancel();
                routes.Watch.Cancel();

                Console.WriteLine($"Stream finalized {streamId}");
            }
        }

        private class WatchAndNounce
        {
            public WatchAndNounce()
            {
                Watch = Watch.Empty;
            }

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