using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using Envoy.Api.V2;
using Envoy.Api.V2.Endpoint;
using Envoy.Api.V2.Route;
using Envoy.ControlPlane.Cache;
using Envoy.ControlPlane.Server;
using Envoy.Service.Discovery.V2;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Logging;
using YamlDotNet.Serialization;

namespace Sample
{
    class Program
    {
        const int Port = 50051;

        public static void Main(string[] args)
        {
            var cancelationTokenSource = new CancellationTokenSource();
            var logger = new ConsoleLogger();
            var cache = new SnapshotCache(true, logger);
            
            var watcher = new SnapshotFileWatcher(s => cache.SetSnapshot("envoy1", s));
            Console.WriteLine("Configuration loaded from files");

            watcher.Start();
            Console.WriteLine("File watcher started");
            
            var services = new Services(cache, logger, cancelationTokenSource.Token);
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
                Ports = { new ServerPort("0.0.0.0", Port, ServerCredentials.Insecure) }
            };
            
            server.Start();

            Console.WriteLine($"Server listening on port {Port.ToString()}");
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            cancelationTokenSource.Cancel();
            server.ShutdownAsync().Wait();
        }
       
    }

    public class SnapshotFileWatcher
    {
        private readonly Action<Snapshot> _onChanged;
        private readonly Deserializer _deserializer = new Deserializer();
        private readonly Newtonsoft.Json.JsonSerializer _serializer = new Newtonsoft.Json.JsonSerializer();
        private readonly JsonParser _jsonParser = 
            new JsonParser(new JsonParser.Settings(10, TypeRegistry.FromMessages(Cluster.Descriptor, ClusterLoadAssignment.Descriptor, RouteConfiguration.Descriptor, Listener.Descriptor)));

        private FileSystemWatcher _watcher;
        private Snapshot _currentSnapshot;
        private int _version = 0;

        public SnapshotFileWatcher(Action<Snapshot> onChanged)
        {
            _onChanged = onChanged;
            _watcher = new FileSystemWatcher
            {
                Path = "config",
                NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite,
                Filter = "*.yaml"
            };
            
            _watcher.Changed += OnChanged;
            
            _currentSnapshot = new Snapshot(null, null, null, null);
            
            ReadConfigurationFile("config/cds.yaml");
            ReadConfigurationFile("config/eds.yaml");
            ReadConfigurationFile("config/lds.yaml");
            ReadConfigurationFile("config/rds.yaml");
        }
        
        public void Start()
        {
            _watcher.EnableRaisingEvents = true;
            _onChanged(_currentSnapshot);
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath.EndsWith("bootstrap.yaml"))
            {
                return;    
            }

            ReadConfigurationFile(e.FullPath);
            _onChanged(_currentSnapshot);
        }

        private void ReadConfigurationFile(string path)
        {
            var yaml = File.ReadAllText(path);
            var json = ConvertYamlToJson(yaml);
            var response = ConvertJsonToProtobuf(json);

            switch (response.Resources[0].TypeUrl)
            {
                case TypeStrings.ClusterType:
                    var clusters = response.Resources.Select(r => r.Unpack<Cluster>());
                    var clustersDictionary = clusters.ToImmutableDictionary(c => c.Name, c => (IMessage) c);
                    _currentSnapshot =
                        _currentSnapshot.WithClusters(
                            new Resources(Interlocked.Increment(ref _version).ToString(),
                                clustersDictionary));
                    break;
                case TypeStrings.EndpointType:
                    var endpoints = response.Resources.Select(r => r.Unpack<ClusterLoadAssignment>());
                    var endpointsDictionary = endpoints.ToImmutableDictionary(c => c.ClusterName, c => (IMessage) c);
                    _currentSnapshot =
                        _currentSnapshot.WithEndpoints(
                            new Resources(Interlocked.Increment(ref _version).ToString(),
                                endpointsDictionary));
                    break;
                case TypeStrings.ListenerType:
                    var listeners = response.Resources.Select(r => r.Unpack<Listener>());
                    var listenersDictionary = listeners.ToImmutableDictionary(c => c.Name, c => (IMessage) c);
                    _currentSnapshot =
                        _currentSnapshot.WithListiners(
                            new Resources(Interlocked.Increment(ref _version).ToString(),
                                listenersDictionary));
                    break;
                case TypeStrings.RouteType:
                    var routes = response.Resources.Select(r => r.Unpack<RouteConfiguration>());
                    var routesDictionary = routes.ToImmutableDictionary(c => c.Name, c => (IMessage) c);
                    _currentSnapshot =
                        _currentSnapshot.WithRoutes(
                            new Resources(Interlocked.Increment(ref _version).ToString(),
                                routesDictionary));
                    break;
            }
        }

        private DiscoveryResponse ConvertJsonToProtobuf(string json)
        {
            return _jsonParser.Parse<DiscoveryResponse>(json);
        }
        
        private string ConvertYamlToJson(string yaml)
        {
            var yamlObject = _deserializer.Deserialize(yaml, typeof(object));

            using (var sw = new StringWriter())
            {
                _serializer.Serialize(sw, yamlObject);
                return sw.ToString();
            }
        } 
    }
}