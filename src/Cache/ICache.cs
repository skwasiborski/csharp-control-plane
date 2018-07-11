using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Envoy.Api.V2;

namespace Envoy.ControlPlane.Cache
{
    public interface ICache
    {
        /// <summary>
        /// Returned task resolves when cahce has values newer (or more resources) that passed in request
        /// </summary>
        Watch CreateWatch(DiscoveryRequest request);

        /// <summary>
        /// Returns current values from cache
        /// </summary>
        ValueTask<DiscoveryResponse> Fetch(DiscoveryRequest request);
    }

    public struct Watch
    {
        internal static readonly Action NoOp = () => { }; 
        private readonly Action _cancel;
        
        public static readonly Watch Empty = new Watch(new TaskCompletionSource<DiscoveryResponse>().Task, NoOp);
        
        public Watch(Task<DiscoveryResponse> response, Action cancel)
        {
            Response = response;
            _cancel = cancel;
        }
        
        public Task<DiscoveryResponse> Response { get; }

        public void Cancel()
        {
            _cancel();
        }
    }
    
    public static class TypeStrings
    {
        public const string TypePrefix = "type.googleapis.com/envoy.api.v2.";
        public const string EndpointType = TypePrefix + "ClusterLoadAssignment";
        public const string ClusterType = TypePrefix + "Cluster";
        public const string RouteType = TypePrefix + "RouteConfiguration";
        public const string ListenerType = TypePrefix + "Listener";
        public const string Any = "";
    }
}