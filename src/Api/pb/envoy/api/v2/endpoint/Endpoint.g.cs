// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: envoy/api/v2/endpoint/endpoint.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Envoy.Api.V2.Endpoint {

  /// <summary>Holder for reflection information generated from envoy/api/v2/endpoint/endpoint.proto</summary>
  public static partial class EndpointReflection {

    #region Descriptor
    /// <summary>File descriptor for envoy/api/v2/endpoint/endpoint.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static EndpointReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CiRlbnZveS9hcGkvdjIvZW5kcG9pbnQvZW5kcG9pbnQucHJvdG8SFWVudm95",
            "LmFwaS52Mi5lbmRwb2ludBofZW52b3kvYXBpL3YyL2NvcmUvYWRkcmVzcy5w",
            "cm90bxocZW52b3kvYXBpL3YyL2NvcmUvYmFzZS5wcm90bxokZW52b3kvYXBp",
            "L3YyL2NvcmUvaGVhbHRoX2NoZWNrLnByb3RvGh5nb29nbGUvcHJvdG9idWYv",
            "d3JhcHBlcnMucHJvdG8aF3ZhbGlkYXRlL3ZhbGlkYXRlLnByb3RvGhRnb2dv",
            "cHJvdG8vZ29nby5wcm90byKwAQoIRW5kcG9pbnQSKwoHYWRkcmVzcxgBIAEo",
            "CzIaLmVudm95LmFwaS52Mi5jb3JlLkFkZHJlc3MSTgoTaGVhbHRoX2NoZWNr",
            "X2NvbmZpZxgCIAEoCzIxLmVudm95LmFwaS52Mi5lbmRwb2ludC5FbmRwb2lu",
            "dC5IZWFsdGhDaGVja0NvbmZpZxonChFIZWFsdGhDaGVja0NvbmZpZxISCgpw",
            "b3J0X3ZhbHVlGAEgASgNIvEBCgpMYkVuZHBvaW50EjEKCGVuZHBvaW50GAEg",
            "ASgLMh8uZW52b3kuYXBpLnYyLmVuZHBvaW50LkVuZHBvaW50EjYKDWhlYWx0",
            "aF9zdGF0dXMYAiABKA4yHy5lbnZveS5hcGkudjIuY29yZS5IZWFsdGhTdGF0",
            "dXMSLQoIbWV0YWRhdGEYAyABKAsyGy5lbnZveS5hcGkudjIuY29yZS5NZXRh",
            "ZGF0YRJJChVsb2FkX2JhbGFuY2luZ193ZWlnaHQYBCABKAsyHC5nb29nbGUu",
            "cHJvdG9idWYuVUludDMyVmFsdWVCDLrpwAMHKgUYgAEoASLgAQoTTG9jYWxp",
            "dHlMYkVuZHBvaW50cxItCghsb2NhbGl0eRgBIAEoCzIbLmVudm95LmFwaS52",
            "Mi5jb3JlLkxvY2FsaXR5Ej0KDGxiX2VuZHBvaW50cxgCIAMoCzIhLmVudm95",
            "LmFwaS52Mi5lbmRwb2ludC5MYkVuZHBvaW50QgTI3h8AEkkKFWxvYWRfYmFs",
            "YW5jaW5nX3dlaWdodBgDIAEoCzIcLmdvb2dsZS5wcm90b2J1Zi5VSW50MzJW",
            "YWx1ZUIMuunAAwcqBRiAASgBEhAKCHByaW9yaXR5GAUgASgNQg5aCGVuZHBv",
            "aW50qOIeAWIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Envoy.Api.V2.Core.AddressReflection.Descriptor, global::Envoy.Api.V2.Core.BaseReflection.Descriptor, global::Envoy.Api.V2.Core.HealthCheckReflection.Descriptor, global::Google.Protobuf.WellKnownTypes.WrappersReflection.Descriptor, global::Validate.ValidateReflection.Descriptor, global::Gogoproto.GogoReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Envoy.Api.V2.Endpoint.Endpoint), global::Envoy.Api.V2.Endpoint.Endpoint.Parser, new[]{ "Address", "HealthCheckConfig" }, null, null, new pbr::GeneratedClrTypeInfo[] { new pbr::GeneratedClrTypeInfo(typeof(global::Envoy.Api.V2.Endpoint.Endpoint.Types.HealthCheckConfig), global::Envoy.Api.V2.Endpoint.Endpoint.Types.HealthCheckConfig.Parser, new[]{ "PortValue" }, null, null, null)}),
            new pbr::GeneratedClrTypeInfo(typeof(global::Envoy.Api.V2.Endpoint.LbEndpoint), global::Envoy.Api.V2.Endpoint.LbEndpoint.Parser, new[]{ "Endpoint", "HealthStatus", "Metadata", "LoadBalancingWeight" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Envoy.Api.V2.Endpoint.LocalityLbEndpoints), global::Envoy.Api.V2.Endpoint.LocalityLbEndpoints.Parser, new[]{ "Locality", "LbEndpoints", "LoadBalancingWeight", "Priority" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  /// Upstream host identifier.
  /// </summary>
  public sealed partial class Endpoint : pb::IMessage<Endpoint> {
    private static readonly pb::MessageParser<Endpoint> _parser = new pb::MessageParser<Endpoint>(() => new Endpoint());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Endpoint> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Envoy.Api.V2.Endpoint.EndpointReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Endpoint() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Endpoint(Endpoint other) : this() {
      Address = other.address_ != null ? other.Address.Clone() : null;
      HealthCheckConfig = other.healthCheckConfig_ != null ? other.HealthCheckConfig.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Endpoint Clone() {
      return new Endpoint(this);
    }

    /// <summary>Field number for the "address" field.</summary>
    public const int AddressFieldNumber = 1;
    private global::Envoy.Api.V2.Core.Address address_;
    /// <summary>
    /// The upstream host address.
    ///
    /// .. attention::
    ///
    ///   The form of host address depends on the given cluster type. For STATIC or EDS,
    ///   it is expected to be a direct IP address (or something resolvable by the
    ///   specified :ref:`resolver &lt;envoy_api_field_core.SocketAddress.resolver_name>`
    ///   in the Address). For LOGICAL or STRICT DNS, it is expected to be hostname,
    ///   and will be resolved via DNS.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Envoy.Api.V2.Core.Address Address {
      get { return address_; }
      set {
        address_ = value;
      }
    }

    /// <summary>Field number for the "health_check_config" field.</summary>
    public const int HealthCheckConfigFieldNumber = 2;
    private global::Envoy.Api.V2.Endpoint.Endpoint.Types.HealthCheckConfig healthCheckConfig_;
    /// <summary>
    /// [#not-implemented-hide:] The optional health check configuration is used as
    /// configuration for the health checker to contact the health checked host.
    ///
    /// .. attention::
    ///
    ///   This takes into effect only for upstream clusters with
    ///   :ref:`active health checking &lt;arch_overview_health_checking>` enabled.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Envoy.Api.V2.Endpoint.Endpoint.Types.HealthCheckConfig HealthCheckConfig {
      get { return healthCheckConfig_; }
      set {
        healthCheckConfig_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Endpoint);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Endpoint other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Address, other.Address)) return false;
      if (!object.Equals(HealthCheckConfig, other.HealthCheckConfig)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (address_ != null) hash ^= Address.GetHashCode();
      if (healthCheckConfig_ != null) hash ^= HealthCheckConfig.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (address_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Address);
      }
      if (healthCheckConfig_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(HealthCheckConfig);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (address_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Address);
      }
      if (healthCheckConfig_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(HealthCheckConfig);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Endpoint other) {
      if (other == null) {
        return;
      }
      if (other.address_ != null) {
        if (address_ == null) {
          address_ = new global::Envoy.Api.V2.Core.Address();
        }
        Address.MergeFrom(other.Address);
      }
      if (other.healthCheckConfig_ != null) {
        if (healthCheckConfig_ == null) {
          healthCheckConfig_ = new global::Envoy.Api.V2.Endpoint.Endpoint.Types.HealthCheckConfig();
        }
        HealthCheckConfig.MergeFrom(other.HealthCheckConfig);
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            if (address_ == null) {
              address_ = new global::Envoy.Api.V2.Core.Address();
            }
            input.ReadMessage(address_);
            break;
          }
          case 18: {
            if (healthCheckConfig_ == null) {
              healthCheckConfig_ = new global::Envoy.Api.V2.Endpoint.Endpoint.Types.HealthCheckConfig();
            }
            input.ReadMessage(healthCheckConfig_);
            break;
          }
        }
      }
    }

    #region Nested types
    /// <summary>Container for nested types declared in the Endpoint message type.</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static partial class Types {
      /// <summary>
      /// [#not-implemented-hide:] The optional health check configuration.
      /// </summary>
      public sealed partial class HealthCheckConfig : pb::IMessage<HealthCheckConfig> {
        private static readonly pb::MessageParser<HealthCheckConfig> _parser = new pb::MessageParser<HealthCheckConfig>(() => new HealthCheckConfig());
        private pb::UnknownFieldSet _unknownFields;
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public static pb::MessageParser<HealthCheckConfig> Parser { get { return _parser; } }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public static pbr::MessageDescriptor Descriptor {
          get { return global::Envoy.Api.V2.Endpoint.Endpoint.Descriptor.NestedTypes[0]; }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        pbr::MessageDescriptor pb::IMessage.Descriptor {
          get { return Descriptor; }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public HealthCheckConfig() {
          OnConstruction();
        }

        partial void OnConstruction();

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public HealthCheckConfig(HealthCheckConfig other) : this() {
          portValue_ = other.portValue_;
          _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public HealthCheckConfig Clone() {
          return new HealthCheckConfig(this);
        }

        /// <summary>Field number for the "port_value" field.</summary>
        public const int PortValueFieldNumber = 1;
        private uint portValue_;
        /// <summary>
        /// Optional alternative health check port value.
        ///
        /// By default the health check address port of an upstream host is the same
        /// as the host's serving address port. This provides an alternative health
        /// check port. Setting this with a non-zero value allows an upstream host
        /// to have different health check address port.
        /// </summary>
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public uint PortValue {
          get { return portValue_; }
          set {
            portValue_ = value;
          }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public override bool Equals(object other) {
          return Equals(other as HealthCheckConfig);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public bool Equals(HealthCheckConfig other) {
          if (ReferenceEquals(other, null)) {
            return false;
          }
          if (ReferenceEquals(other, this)) {
            return true;
          }
          if (PortValue != other.PortValue) return false;
          return Equals(_unknownFields, other._unknownFields);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public override int GetHashCode() {
          int hash = 1;
          if (PortValue != 0) hash ^= PortValue.GetHashCode();
          if (_unknownFields != null) {
            hash ^= _unknownFields.GetHashCode();
          }
          return hash;
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public override string ToString() {
          return pb::JsonFormatter.ToDiagnosticString(this);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public void WriteTo(pb::CodedOutputStream output) {
          if (PortValue != 0) {
            output.WriteRawTag(8);
            output.WriteUInt32(PortValue);
          }
          if (_unknownFields != null) {
            _unknownFields.WriteTo(output);
          }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public int CalculateSize() {
          int size = 0;
          if (PortValue != 0) {
            size += 1 + pb::CodedOutputStream.ComputeUInt32Size(PortValue);
          }
          if (_unknownFields != null) {
            size += _unknownFields.CalculateSize();
          }
          return size;
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public void MergeFrom(HealthCheckConfig other) {
          if (other == null) {
            return;
          }
          if (other.PortValue != 0) {
            PortValue = other.PortValue;
          }
          _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public void MergeFrom(pb::CodedInputStream input) {
          uint tag;
          while ((tag = input.ReadTag()) != 0) {
            switch(tag) {
              default:
                _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
                break;
              case 8: {
                PortValue = input.ReadUInt32();
                break;
              }
            }
          }
        }

      }

    }
    #endregion

  }

  /// <summary>
  /// An Endpoint that Envoy can route traffic to.
  /// </summary>
  public sealed partial class LbEndpoint : pb::IMessage<LbEndpoint> {
    private static readonly pb::MessageParser<LbEndpoint> _parser = new pb::MessageParser<LbEndpoint>(() => new LbEndpoint());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<LbEndpoint> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Envoy.Api.V2.Endpoint.EndpointReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public LbEndpoint() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public LbEndpoint(LbEndpoint other) : this() {
      Endpoint = other.endpoint_ != null ? other.Endpoint.Clone() : null;
      healthStatus_ = other.healthStatus_;
      Metadata = other.metadata_ != null ? other.Metadata.Clone() : null;
      LoadBalancingWeight = other.LoadBalancingWeight;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public LbEndpoint Clone() {
      return new LbEndpoint(this);
    }

    /// <summary>Field number for the "endpoint" field.</summary>
    public const int EndpointFieldNumber = 1;
    private global::Envoy.Api.V2.Endpoint.Endpoint endpoint_;
    /// <summary>
    /// Upstream host identifier
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Envoy.Api.V2.Endpoint.Endpoint Endpoint {
      get { return endpoint_; }
      set {
        endpoint_ = value;
      }
    }

    /// <summary>Field number for the "health_status" field.</summary>
    public const int HealthStatusFieldNumber = 2;
    private global::Envoy.Api.V2.Core.HealthStatus healthStatus_ = 0;
    /// <summary>
    /// Optional health status when known and supplied by EDS server.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Envoy.Api.V2.Core.HealthStatus HealthStatus {
      get { return healthStatus_; }
      set {
        healthStatus_ = value;
      }
    }

    /// <summary>Field number for the "metadata" field.</summary>
    public const int MetadataFieldNumber = 3;
    private global::Envoy.Api.V2.Core.Metadata metadata_;
    /// <summary>
    /// The endpoint metadata specifies values that may be used by the load
    /// balancer to select endpoints in a cluster for a given request. The filter
    /// name should be specified as *envoy.lb*. An example boolean key-value pair
    /// is *canary*, providing the optional canary status of the upstream host.
    /// This may be matched against in a route's ForwardAction metadata_match field
    /// to subset the endpoints considered in cluster load balancing.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Envoy.Api.V2.Core.Metadata Metadata {
      get { return metadata_; }
      set {
        metadata_ = value;
      }
    }

    /// <summary>Field number for the "load_balancing_weight" field.</summary>
    public const int LoadBalancingWeightFieldNumber = 4;
    private static readonly pb::FieldCodec<uint?> _single_loadBalancingWeight_codec = pb::FieldCodec.ForStructWrapper<uint>(34);
    private uint? loadBalancingWeight_;
    /// <summary>
    /// The optional load balancing weight of the upstream host, in the range 1 -
    /// 128. Envoy uses the load balancing weight in some of the built in load
    /// balancers. The load balancing weight for an endpoint is divided by the sum
    /// of the weights of all endpoints in the endpoint's locality to produce a
    /// percentage of traffic for the endpoint. This percentage is then further
    /// weighted by the endpoint's locality's load balancing weight from
    /// LocalityLbEndpoints. If unspecified, each host is presumed to have equal
    /// weight in a locality.
    ///
    /// .. attention::
    ///
    ///   The limit of 128 is somewhat arbitrary, but is applied due to performance
    ///   concerns with the current implementation and can be removed when
    ///   `this issue &lt;https://github.com/envoyproxy/envoy/issues/1285>`_ is fixed.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint? LoadBalancingWeight {
      get { return loadBalancingWeight_; }
      set {
        loadBalancingWeight_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as LbEndpoint);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(LbEndpoint other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Endpoint, other.Endpoint)) return false;
      if (HealthStatus != other.HealthStatus) return false;
      if (!object.Equals(Metadata, other.Metadata)) return false;
      if (LoadBalancingWeight != other.LoadBalancingWeight) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (endpoint_ != null) hash ^= Endpoint.GetHashCode();
      if (HealthStatus != 0) hash ^= HealthStatus.GetHashCode();
      if (metadata_ != null) hash ^= Metadata.GetHashCode();
      if (loadBalancingWeight_ != null) hash ^= LoadBalancingWeight.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (endpoint_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Endpoint);
      }
      if (HealthStatus != 0) {
        output.WriteRawTag(16);
        output.WriteEnum((int) HealthStatus);
      }
      if (metadata_ != null) {
        output.WriteRawTag(26);
        output.WriteMessage(Metadata);
      }
      if (loadBalancingWeight_ != null) {
        _single_loadBalancingWeight_codec.WriteTagAndValue(output, LoadBalancingWeight);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (endpoint_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Endpoint);
      }
      if (HealthStatus != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) HealthStatus);
      }
      if (metadata_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Metadata);
      }
      if (loadBalancingWeight_ != null) {
        size += _single_loadBalancingWeight_codec.CalculateSizeWithTag(LoadBalancingWeight);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(LbEndpoint other) {
      if (other == null) {
        return;
      }
      if (other.endpoint_ != null) {
        if (endpoint_ == null) {
          endpoint_ = new global::Envoy.Api.V2.Endpoint.Endpoint();
        }
        Endpoint.MergeFrom(other.Endpoint);
      }
      if (other.HealthStatus != 0) {
        HealthStatus = other.HealthStatus;
      }
      if (other.metadata_ != null) {
        if (metadata_ == null) {
          metadata_ = new global::Envoy.Api.V2.Core.Metadata();
        }
        Metadata.MergeFrom(other.Metadata);
      }
      if (other.loadBalancingWeight_ != null) {
        if (loadBalancingWeight_ == null || other.LoadBalancingWeight != 0) {
          LoadBalancingWeight = other.LoadBalancingWeight;
        }
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            if (endpoint_ == null) {
              endpoint_ = new global::Envoy.Api.V2.Endpoint.Endpoint();
            }
            input.ReadMessage(endpoint_);
            break;
          }
          case 16: {
            healthStatus_ = (global::Envoy.Api.V2.Core.HealthStatus) input.ReadEnum();
            break;
          }
          case 26: {
            if (metadata_ == null) {
              metadata_ = new global::Envoy.Api.V2.Core.Metadata();
            }
            input.ReadMessage(metadata_);
            break;
          }
          case 34: {
            uint? value = _single_loadBalancingWeight_codec.Read(input);
            if (loadBalancingWeight_ == null || value != 0) {
              LoadBalancingWeight = value;
            }
            break;
          }
        }
      }
    }

  }

  /// <summary>
  /// A group of endpoints belonging to a Locality.
  /// One can have multiple LocalityLbEndpoints for a locality, but this is
  /// generally only done if the different groups need to have different load
  /// balancing weights or different priorities.
  /// </summary>
  public sealed partial class LocalityLbEndpoints : pb::IMessage<LocalityLbEndpoints> {
    private static readonly pb::MessageParser<LocalityLbEndpoints> _parser = new pb::MessageParser<LocalityLbEndpoints>(() => new LocalityLbEndpoints());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<LocalityLbEndpoints> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Envoy.Api.V2.Endpoint.EndpointReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public LocalityLbEndpoints() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public LocalityLbEndpoints(LocalityLbEndpoints other) : this() {
      Locality = other.locality_ != null ? other.Locality.Clone() : null;
      lbEndpoints_ = other.lbEndpoints_.Clone();
      LoadBalancingWeight = other.LoadBalancingWeight;
      priority_ = other.priority_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public LocalityLbEndpoints Clone() {
      return new LocalityLbEndpoints(this);
    }

    /// <summary>Field number for the "locality" field.</summary>
    public const int LocalityFieldNumber = 1;
    private global::Envoy.Api.V2.Core.Locality locality_;
    /// <summary>
    /// Identifies location of where the upstream hosts run.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Envoy.Api.V2.Core.Locality Locality {
      get { return locality_; }
      set {
        locality_ = value;
      }
    }

    /// <summary>Field number for the "lb_endpoints" field.</summary>
    public const int LbEndpointsFieldNumber = 2;
    private static readonly pb::FieldCodec<global::Envoy.Api.V2.Endpoint.LbEndpoint> _repeated_lbEndpoints_codec
        = pb::FieldCodec.ForMessage(18, global::Envoy.Api.V2.Endpoint.LbEndpoint.Parser);
    private readonly pbc::RepeatedField<global::Envoy.Api.V2.Endpoint.LbEndpoint> lbEndpoints_ = new pbc::RepeatedField<global::Envoy.Api.V2.Endpoint.LbEndpoint>();
    /// <summary>
    /// The group of endpoints belonging to the locality specified.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Envoy.Api.V2.Endpoint.LbEndpoint> LbEndpoints {
      get { return lbEndpoints_; }
    }

    /// <summary>Field number for the "load_balancing_weight" field.</summary>
    public const int LoadBalancingWeightFieldNumber = 3;
    private static readonly pb::FieldCodec<uint?> _single_loadBalancingWeight_codec = pb::FieldCodec.ForStructWrapper<uint>(26);
    private uint? loadBalancingWeight_;
    /// <summary>
    /// Optional: Per priority/region/zone/sub_zone weight - range 1-128. The load
    /// balancing weight for a locality is divided by the sum of the weights of all
    /// localities  at the same priority level to produce the effective percentage
    /// of traffic for the locality.
    ///
    /// Locality weights are only considered when :ref:`locality weighted load
    /// balancing &lt;arch_overview_load_balancing_locality_weighted_lb>` is
    /// configured. These weights are ignored otherwise. If no weights are
    /// specificed when locality weighted load balancing is enabled, the cluster is
    /// assumed to have a weight of 1.
    ///
    /// .. attention::
    ///
    ///   The limit of 128 is somewhat arbitrary, but is applied due to performance
    ///   concerns with the current implementation and can be removed when
    ///   `this issue &lt;https://github.com/envoyproxy/envoy/issues/1285>`_ is fixed.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint? LoadBalancingWeight {
      get { return loadBalancingWeight_; }
      set {
        loadBalancingWeight_ = value;
      }
    }

    /// <summary>Field number for the "priority" field.</summary>
    public const int PriorityFieldNumber = 5;
    private uint priority_;
    /// <summary>
    /// Optional: the priority for this LocalityLbEndpoints. If unspecified this will
    /// default to the highest priority (0).
    ///
    /// Under usual circumstances, Envoy will only select endpoints for the highest
    /// priority (0). In the event all endpoints for a particular priority are
    /// unavailable/unhealthy, Envoy will fail over to selecting endpoints for the
    /// next highest priority group.
    ///
    /// Priorities should range from 0 (highest) to N (lowest) without skipping.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint Priority {
      get { return priority_; }
      set {
        priority_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as LocalityLbEndpoints);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(LocalityLbEndpoints other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Locality, other.Locality)) return false;
      if(!lbEndpoints_.Equals(other.lbEndpoints_)) return false;
      if (LoadBalancingWeight != other.LoadBalancingWeight) return false;
      if (Priority != other.Priority) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (locality_ != null) hash ^= Locality.GetHashCode();
      hash ^= lbEndpoints_.GetHashCode();
      if (loadBalancingWeight_ != null) hash ^= LoadBalancingWeight.GetHashCode();
      if (Priority != 0) hash ^= Priority.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (locality_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Locality);
      }
      lbEndpoints_.WriteTo(output, _repeated_lbEndpoints_codec);
      if (loadBalancingWeight_ != null) {
        _single_loadBalancingWeight_codec.WriteTagAndValue(output, LoadBalancingWeight);
      }
      if (Priority != 0) {
        output.WriteRawTag(40);
        output.WriteUInt32(Priority);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (locality_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Locality);
      }
      size += lbEndpoints_.CalculateSize(_repeated_lbEndpoints_codec);
      if (loadBalancingWeight_ != null) {
        size += _single_loadBalancingWeight_codec.CalculateSizeWithTag(LoadBalancingWeight);
      }
      if (Priority != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(Priority);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(LocalityLbEndpoints other) {
      if (other == null) {
        return;
      }
      if (other.locality_ != null) {
        if (locality_ == null) {
          locality_ = new global::Envoy.Api.V2.Core.Locality();
        }
        Locality.MergeFrom(other.Locality);
      }
      lbEndpoints_.Add(other.lbEndpoints_);
      if (other.loadBalancingWeight_ != null) {
        if (loadBalancingWeight_ == null || other.LoadBalancingWeight != 0) {
          LoadBalancingWeight = other.LoadBalancingWeight;
        }
      }
      if (other.Priority != 0) {
        Priority = other.Priority;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            if (locality_ == null) {
              locality_ = new global::Envoy.Api.V2.Core.Locality();
            }
            input.ReadMessage(locality_);
            break;
          }
          case 18: {
            lbEndpoints_.AddEntriesFrom(input, _repeated_lbEndpoints_codec);
            break;
          }
          case 26: {
            uint? value = _single_loadBalancingWeight_codec.Read(input);
            if (loadBalancingWeight_ == null || value != 0) {
              LoadBalancingWeight = value;
            }
            break;
          }
          case 40: {
            Priority = input.ReadUInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
