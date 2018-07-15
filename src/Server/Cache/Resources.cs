using System.Collections.Immutable;
using Google.Protobuf;

namespace Envoy.ControlPlane.Server.Cache
{
    public class Resources
    {
        public string Version { get; }
        public ImmutableDictionary<string, IMessage> Items { get; }

        public Resources(string version, ImmutableDictionary<string, IMessage> items)
        {
            Items = items;
            Version = version;
        }
    }
}