using System.Collections.Generic;
using System.Collections.Immutable;
using Google.Protobuf;

namespace Envoy.ControlPlane.Server.Cache
{
    public class Resources
    {
        public string Version { get; }
        public IDictionary<string, IMessage> Items { get; }

        public Resources(string version, IDictionary<string, IMessage> items)
        {
            Items = items;
            Version = version;
        }
    }
}