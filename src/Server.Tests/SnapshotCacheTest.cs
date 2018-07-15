using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Envoy.Api.V2;
using Envoy.Api.V2.Core;
using Envoy.ControlPlane.Server.Cache;
using Google.Protobuf;
using Grpc.Core.Logging;
using Xunit;

namespace Envoy.ControlPlane.Server.Tests
{
    public class SnapshotCacheTest
    {
        public static IEnumerable<object[]> ClusterRequest()
        {
            object[] GenerateSingleTypeRequests(string typeUrl)
            {
                var result = new object[3];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = new DiscoveryRequest()
                    {
                        Node = new Node() {Id = "node1"},
                        TypeUrl = typeUrl,
                        VersionInfo = i == 0 ? string.Empty : i.ToString()
                    };
                }

                return result;
            }
            
            yield return GenerateSingleTypeRequests(TypeStrings.ClusterType);
            yield return GenerateSingleTypeRequests(TypeStrings.EndpointType);
            yield return GenerateSingleTypeRequests(TypeStrings.ListenerType);
            yield return GenerateSingleTypeRequests(TypeStrings.RouteType);
        }

        private Snapshot BuildSnapshot(string version)
        {
            var clustersBuilder = ImmutableDictionary.CreateBuilder<string, IMessage>();
            clustersBuilder.Add("c1", new Cluster());
            
            var endpointBuilder = ImmutableDictionary.CreateBuilder<string, IMessage>();
            endpointBuilder.Add("e1", new ClusterLoadAssignment());
            
            var listenerBuilder = ImmutableDictionary.CreateBuilder<string, IMessage>();
            listenerBuilder.Add("l1", new Listener());
            
            var routeBuilder = ImmutableDictionary.CreateBuilder<string, IMessage>();
            routeBuilder.Add("r1", new RouteConfiguration());
            
            return new Snapshot(
                new Resources(version, endpointBuilder.ToImmutable()), 
                new Resources(version, clustersBuilder.ToImmutable()), 
                new Resources(version, routeBuilder.ToImmutable()), 
                new Resources(version, listenerBuilder.ToImmutable()));
        }

        [Theory]
        [MemberData(nameof(ClusterRequest))]
        public async Task NoSnapshot_GetResponseForFetch_ReturnsEmpty(
            DiscoveryRequest requests0,
            DiscoveryRequest requests1,
            DiscoveryRequest requests2)
        {
            // Arrange
            var cache = new SnapshotCache(true, new NullLogger());

            // Act
            var result = await cache.Fetch(requests0);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [MemberData(nameof(ClusterRequest))]
        public async Task NoSnapshot_GetResponseForStream_ReturnsUnresolvedTask(            
            DiscoveryRequest requests0,
            DiscoveryRequest requests1,
            DiscoveryRequest requests2)
        {
            // Arrange
            var cache = new SnapshotCache(true, new NullLogger());

            // Act
            var resultTask = cache.CreateWatch(requests0);

            // Assert
            await Task.Delay(100);
            Assert.False(resultTask.Response.IsCompleted);
        }

        [Theory]
        [MemberData(nameof(ClusterRequest))]
        public async Task Snapshot_GetResponseForWithTheSameVersionStream_ReturnsNotResolvedTask(            
            DiscoveryRequest requests0,
            DiscoveryRequest requests1,
            DiscoveryRequest requests2)
        {
            // Arrange
            var cache = new SnapshotCache(true, new NullLogger());
            cache.SetSnapshot("node1", BuildSnapshot("2"));

            // Act
            var resultTask = cache.CreateWatch(requests2);

            // Assert
            await Task.Delay(100);
            Assert.False(resultTask.Response.IsCompleted);
        }
        
        [Theory]
        [MemberData(nameof(ClusterRequest))]
        public async Task Snapshot_GetResponseForStream_ReturnsResolvedTask(            
            DiscoveryRequest requests0,
            DiscoveryRequest requests1,
            DiscoveryRequest requests2)
        {
            // Arrange
            var cache = new SnapshotCache(true, new NullLogger());
            cache.SetSnapshot("node1", BuildSnapshot("2"));

            // Act
            var result = await cache.CreateWatch(requests0).Response;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("2", result.VersionInfo);
            Assert.Equal(requests0.TypeUrl, result.TypeUrl);
        }

        [Theory]
        [MemberData(nameof(ClusterRequest))]
        public async Task SnapshotAndGetResponseForSameVersionStream_SnapshotWithNewVersionSet_ResolvedTask(            
            DiscoveryRequest requests0,
            DiscoveryRequest requests1,
            DiscoveryRequest requests2)
        {
            // Arrange
            var cache = new SnapshotCache(true, new NullLogger());
            cache.SetSnapshot("node1", BuildSnapshot("1"));
            var resultTask = cache.CreateWatch(requests1).Response;

            // Act
            cache.SetSnapshot("node1", BuildSnapshot("2"));
            var result = await resultTask;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("2", result.VersionInfo);
            Assert.Equal(requests0.TypeUrl, result.TypeUrl);
        }
    }
}