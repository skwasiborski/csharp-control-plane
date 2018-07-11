using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using Envoy.Api.V2;
using Envoy.ControlPlane.Cache;
using Envoy.ControlPlane.Server;
using Envoy.Service.Discovery.V2;
using Grpc.Core;

namespace Sample
{
    class Program
    {
        const int Port = 50051;

        public static async Task Main(string[] args)
        {
            var cancelationTokenSource = new CancellationTokenSource();
            var services = new Services(new SimpleCache(), cancelationTokenSource.Token);
            Server server = new Server
            {
                Services =
                {
                    ClusterDiscoveryService.BindService(services.ClusterService),
                    EndpointDiscoveryService.BindService(services.EndpointService),
                    ListenerDiscoveryService.BindService(services.ListenerService),
                    RouteDiscoveryService.BindService(services.RouteService),
                    AggregatedDiscoveryService.BindService(services.AggregatedService)
                },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            
            server.Start();

            Console.WriteLine($"ClusterDiscoveryService server listening on port {Port.ToString()}");
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            cancelationTokenSource.Cancel();
            await server.ShutdownAsync();
        }
    }
}