using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Envoy.Api.V2;
using Envoy.ControlPlane.Cache;
using Envoy.Service.Discovery.V2;
using Grpc.Core;
using Grpc.Core.Logging;

namespace Envoy.ControlPlane.Server
{
    public class Services
    {
        private readonly ICache _cache;
        private readonly ILogger _logger;

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

        public Services(ICache cache, ILogger logger, CancellationToken cancellationToken)
        {
            _cache = cache;
            _logger = logger.ForType<Services>();
            _cancellationToken = cancellationToken;
            _clusterService = new ClusterDiscoveryServiceImpl(this);
            _endpointService = new EndpointDiscoveryServiceImpl(this);
            _listenerService = new ListenerDiscoveryServiceImpl(this);
            _routeService = new RouteDiscoveryServiceImpl(this);
            _aggregatedService = new AggregatedDiscoveryServiceImpl(this);
        }

        private async Task HandleStream(
            IAsyncEnumerator<DiscoveryRequest> requestStream,
            IAsyncStreamWriter<DiscoveryResponse> responseStream,
            ServerCallContext context,
            string defaultTypeUrl)
        {
            var streamId = Guid.NewGuid();
            _logger.InfoF($"New Stream started {streamId}");

            var endpoints = new WatchAndNounce();
            var clusters = new WatchAndNounce();
            var listeners = new WatchAndNounce();
            var routes = new WatchAndNounce();
            
            var watches = new Dictionary<string, WatchAndNounce>()
            {
                {TypeStrings.ClusterType, clusters},
                {TypeStrings.EndpointType, endpoints},
                {TypeStrings.ListenerType, listeners},
                {TypeStrings.RouteType, routes},
            };

            try
            {
                var streamNonce = 0;

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
                                _logger.Warning("type URL is required for ADS");
                                throw new InvalidOperationException("type URL is required for ADS");
                            }

                            if (string.IsNullOrEmpty(request.TypeUrl))
                            {
                                request.TypeUrl = defaultTypeUrl;
                            }

                            _logger.DebugF($"<- New request on stream {streamId}, typeUrl {request.TypeUrl}, version {request.VersionInfo}, nonce: {request.ResponseNonce}");

                            var requestWatch = watches[request.TypeUrl];
                            if (requestWatch.Nonce == null || requestWatch.Nonce == request.ResponseNonce)
                            {
                                // if the nonce is not correct ignore the request.
                                requestWatch.Watch.Cancel();
                                requestWatch.Watch = _cache.CreateWatch(request);
                            }

                            requestTask = requestStream.MoveNext(_cancellationToken);
                            break;
                        case Task<DiscoveryResponse> responseTask:
                            // Watch was resolved. Send he update.
                            var response = responseTask.Result;
                            var responseWatch = watches[response.TypeUrl];
                            
                            response.Nonce = (++streamNonce).ToString();
                            responseWatch.Nonce = streamNonce.ToString();

                            // the value is consumed we do not want to get it again from this watch. 
                            responseWatch.Watch.Cancel();
                            responseWatch.Watch = Watch.Empty;

                            _logger.DebugF(
                                $"-> New response on stream {streamId}, typeUrl {response.TypeUrl}, version {response.VersionInfo}, nonce: {response.Nonce}");

                            await responseStream.WriteAsync(response);
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

                _logger.InfoF($"Stream finalized {streamId}");
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