using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using Envoy.Api.V2;
using Envoy.Service.Discovery.V2;
using Grpc.Core;

namespace Sample
{
    class ClusterDiscoveryServiceImpl : ClusterDiscoveryService.ClusterDiscoveryServiceBase
    {
        public override Task StreamClusters(IAsyncStreamReader<DiscoveryRequest> requestStream, IServerStreamWriter<DiscoveryResponse> responseStream, ServerCallContext context)
        {
            return base.StreamClusters(requestStream, responseStream, context);
        }
    }
    
    class EndpointDiscoveryServiceImpl : EndpointDiscoveryService.EndpointDiscoveryServiceBase
    {
        public override Task StreamEndpoints(IAsyncStreamReader<DiscoveryRequest> requestStream, IServerStreamWriter<DiscoveryResponse> responseStream, ServerCallContext context)
        {
            return base.StreamEndpoints(requestStream, responseStream, context);
        }
    }

    class ListenerDiscoveryServiceImpl : ListenerDiscoveryService.ListenerDiscoveryServiceBase
    {
        public override Task StreamListeners(IAsyncStreamReader<DiscoveryRequest> requestStream, IServerStreamWriter<DiscoveryResponse> responseStream, ServerCallContext context)
        {
            return base.StreamListeners(requestStream, responseStream, context);
        }
    }

    class RouteDiscoveryServiceImpl : RouteDiscoveryService.RouteDiscoveryServiceBase
    {
        public override Task StreamRoutes(IAsyncStreamReader<DiscoveryRequest> requestStream, IServerStreamWriter<DiscoveryResponse> responseStream, ServerCallContext context)
        {
            return base.StreamRoutes(requestStream, responseStream, context);
        }
    }    
    
    class AggregatedDiscoveryServiceImpl : AggregatedDiscoveryService.AggregatedDiscoveryServiceBase
    {
        public override Task StreamAggregatedResources(IAsyncStreamReader<DiscoveryRequest> requestStream, IServerStreamWriter<DiscoveryResponse> responseStream,
            ServerCallContext context)
        {
            return base.StreamAggregatedResources(requestStream, responseStream, context);
        }
    }
    
    class Program
    {
        const int Port = 50051;

        public static async Task Main(string[] args)
        {
            Server server = new Server
            {
                Services =
                {
                    ClusterDiscoveryService.BindService(new ClusterDiscoveryServiceImpl()),
                    EndpointDiscoveryService.BindService(new EndpointDiscoveryServiceImpl()),
                    ListenerDiscoveryService.BindService(new ListenerDiscoveryServiceImpl()),
                    RouteDiscoveryService.BindService(new RouteDiscoveryServiceImpl()),
                    AggregatedDiscoveryService.BindService(new AggregatedDiscoveryServiceImpl())
                },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            
            server.Start();

            Console.WriteLine($"ClusterDiscoveryService server listening on port {Port.ToString()}");
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            await server.ShutdownAsync();
        }
    }
}