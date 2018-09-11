using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Google.Protobuf;

namespace Envoy.ControlPlane.Server.Cache
{
    public class Snapshot
    {
        public Resources Endpoints { get; }
        public Resources Clusters { get; }
        public Resources Routes { get; }
        public Resources Listiners { get; }

        public Snapshot(Resources endpoints, Resources clusters, Resources routes, Resources listiners)
        {
            Endpoints = endpoints;
            Clusters = clusters;
            Routes = routes;
            Listiners = listiners;
        }
        
        public Snapshot WithEndpoints(Resources endpoints) => new Snapshot(endpoints, Clusters, Routes, Listiners);
        public Snapshot WithClusters(Resources clusters) => new Snapshot(Endpoints, clusters, Routes, Listiners);
        public Snapshot WithRoutes(Resources routes) => new Snapshot(Endpoints, Clusters, routes, Listiners);
        public Snapshot WithListiners(Resources listiners) => new Snapshot(Endpoints, Clusters, Routes, listiners);

        public string GetVersion(string type)
        {
            return GetByType(type, e => e.Version);
        }

        public IDictionary<string, IMessage> GetResources(string type)
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
}