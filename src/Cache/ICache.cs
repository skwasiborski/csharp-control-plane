using System.Threading.Tasks;
using Envoy.Api.V2;

namespace Cache
{
    public interface ICache
    {
        /// <summary>
        /// Returned task resolves when cahce has values newer (or more resources) that passed in request
        /// </summary>
        ValueTask<DiscoveryResponse> GetResponseForStream(DiscoveryRequest request);

        /// <summary>
        /// Returns current values from cache
        /// </summary>
        ValueTask<DiscoveryResponse> GetResponseForFetch(DiscoveryRequest request);
    }
}