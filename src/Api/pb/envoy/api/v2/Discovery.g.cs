// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: envoy/api/v2/discovery.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Envoy.Api.V2 {

  /// <summary>Holder for reflection information generated from envoy/api/v2/discovery.proto</summary>
  public static partial class DiscoveryReflection {

    #region Descriptor
    /// <summary>File descriptor for envoy/api/v2/discovery.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static DiscoveryReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChxlbnZveS9hcGkvdjIvZGlzY292ZXJ5LnByb3RvEgxlbnZveS5hcGkudjIa",
            "HGVudm95L2FwaS92Mi9jb3JlL2Jhc2UucHJvdG8aGWdvb2dsZS9wcm90b2J1",
            "Zi9hbnkucHJvdG8aF2dvb2dsZS9ycGMvc3RhdHVzLnByb3RvGhRnb2dvcHJv",
            "dG8vZ29nby5wcm90byK7AQoQRGlzY292ZXJ5UmVxdWVzdBIUCgx2ZXJzaW9u",
            "X2luZm8YASABKAkSJQoEbm9kZRgCIAEoCzIXLmVudm95LmFwaS52Mi5jb3Jl",
            "Lk5vZGUSFgoOcmVzb3VyY2VfbmFtZXMYAyADKAkSEAoIdHlwZV91cmwYBCAB",
            "KAkSFgoOcmVzcG9uc2Vfbm9uY2UYBSABKAkSKAoMZXJyb3JfZGV0YWlsGAYg",
            "ASgLMhIuZ29vZ2xlLnJwYy5TdGF0dXMiiQEKEURpc2NvdmVyeVJlc3BvbnNl",
            "EhQKDHZlcnNpb25faW5mbxgBIAEoCRItCglyZXNvdXJjZXMYAiADKAsyFC5n",
            "b29nbGUucHJvdG9idWYuQW55QgTI3h8AEg4KBmNhbmFyeRgDIAEoCBIQCgh0",
            "eXBlX3VybBgEIAEoCRINCgVub25jZRgFIAEoCUIIWgJ2MqjiHgFiBnByb3Rv",
            "Mw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Envoy.Api.V2.Core.BaseReflection.Descriptor, global::Google.Protobuf.WellKnownTypes.AnyReflection.Descriptor, global::Google.Rpc.StatusReflection.Descriptor, global::Gogoproto.GogoReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Envoy.Api.V2.DiscoveryRequest), global::Envoy.Api.V2.DiscoveryRequest.Parser, new[]{ "VersionInfo", "Node", "ResourceNames", "TypeUrl", "ResponseNonce", "ErrorDetail" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Envoy.Api.V2.DiscoveryResponse), global::Envoy.Api.V2.DiscoveryResponse.Parser, new[]{ "VersionInfo", "Resources", "Canary", "TypeUrl", "Nonce" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  /// A DiscoveryRequest requests a set of versioned resources of the same type for
  /// a given Envoy node on some API.
  /// </summary>
  public sealed partial class DiscoveryRequest : pb::IMessage<DiscoveryRequest> {
    private static readonly pb::MessageParser<DiscoveryRequest> _parser = new pb::MessageParser<DiscoveryRequest>(() => new DiscoveryRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<DiscoveryRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Envoy.Api.V2.DiscoveryReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DiscoveryRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DiscoveryRequest(DiscoveryRequest other) : this() {
      versionInfo_ = other.versionInfo_;
      Node = other.node_ != null ? other.Node.Clone() : null;
      resourceNames_ = other.resourceNames_.Clone();
      typeUrl_ = other.typeUrl_;
      responseNonce_ = other.responseNonce_;
      ErrorDetail = other.errorDetail_ != null ? other.ErrorDetail.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DiscoveryRequest Clone() {
      return new DiscoveryRequest(this);
    }

    /// <summary>Field number for the "version_info" field.</summary>
    public const int VersionInfoFieldNumber = 1;
    private string versionInfo_ = "";
    /// <summary>
    /// The version_info provided in the request messages will be the version_info
    /// received with the most recent successfully processed response or empty on
    /// the first request. It is expected that no new request is sent after a
    /// response is received until the Envoy instance is ready to ACK/NACK the new
    /// configuration. ACK/NACK takes place by returning the new API config version
    /// as applied or the previous API config version respectively. Each type_url
    /// (see below) has an independent version associated with it.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string VersionInfo {
      get { return versionInfo_; }
      set {
        versionInfo_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "node" field.</summary>
    public const int NodeFieldNumber = 2;
    private global::Envoy.Api.V2.Core.Node node_;
    /// <summary>
    /// The node making the request.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Envoy.Api.V2.Core.Node Node {
      get { return node_; }
      set {
        node_ = value;
      }
    }

    /// <summary>Field number for the "resource_names" field.</summary>
    public const int ResourceNamesFieldNumber = 3;
    private static readonly pb::FieldCodec<string> _repeated_resourceNames_codec
        = pb::FieldCodec.ForString(26);
    private readonly pbc::RepeatedField<string> resourceNames_ = new pbc::RepeatedField<string>();
    /// <summary>
    /// List of resources to subscribe to, e.g. list of cluster names or a route
    /// configuration name. If this is empty, all resources for the API are
    /// returned. LDS/CDS expect empty resource_names, since this is global
    /// discovery for the Envoy instance. The LDS and CDS responses will then imply
    /// a number of resources that need to be fetched via EDS/RDS, which will be
    /// explicitly enumerated in resource_names.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<string> ResourceNames {
      get { return resourceNames_; }
    }

    /// <summary>Field number for the "type_url" field.</summary>
    public const int TypeUrlFieldNumber = 4;
    private string typeUrl_ = "";
    /// <summary>
    /// Type of the resource that is being requested, e.g.
    /// "type.googleapis.com/envoy.api.v2.ClusterLoadAssignment". This is implicit
    /// in requests made via singleton xDS APIs such as CDS, LDS, etc. but is
    /// required for ADS.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string TypeUrl {
      get { return typeUrl_; }
      set {
        typeUrl_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "response_nonce" field.</summary>
    public const int ResponseNonceFieldNumber = 5;
    private string responseNonce_ = "";
    /// <summary>
    /// nonce corresponding to DiscoveryResponse being ACK/NACKed. See above
    /// discussion on version_info and the DiscoveryResponse nonce comment. This
    /// may be empty if no nonce is available, e.g. at startup or for non-stream
    /// xDS implementations.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string ResponseNonce {
      get { return responseNonce_; }
      set {
        responseNonce_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "error_detail" field.</summary>
    public const int ErrorDetailFieldNumber = 6;
    private global::Google.Rpc.Status errorDetail_;
    /// <summary>
    /// This is populated when the previous :ref:`DiscoveryResponse &lt;envoy_api_msg_DiscoveryResponse>`
    /// failed to update configuration. The *message* field in *error_details* provides the Envoy
    /// internal exception related to the failure. It is only intended for consumption during manual
    /// debugging, the string provided is not guaranteed to be stable across Envoy versions.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Google.Rpc.Status ErrorDetail {
      get { return errorDetail_; }
      set {
        errorDetail_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as DiscoveryRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(DiscoveryRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (VersionInfo != other.VersionInfo) return false;
      if (!object.Equals(Node, other.Node)) return false;
      if(!resourceNames_.Equals(other.resourceNames_)) return false;
      if (TypeUrl != other.TypeUrl) return false;
      if (ResponseNonce != other.ResponseNonce) return false;
      if (!object.Equals(ErrorDetail, other.ErrorDetail)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (VersionInfo.Length != 0) hash ^= VersionInfo.GetHashCode();
      if (node_ != null) hash ^= Node.GetHashCode();
      hash ^= resourceNames_.GetHashCode();
      if (TypeUrl.Length != 0) hash ^= TypeUrl.GetHashCode();
      if (ResponseNonce.Length != 0) hash ^= ResponseNonce.GetHashCode();
      if (errorDetail_ != null) hash ^= ErrorDetail.GetHashCode();
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
      if (VersionInfo.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(VersionInfo);
      }
      if (node_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(Node);
      }
      resourceNames_.WriteTo(output, _repeated_resourceNames_codec);
      if (TypeUrl.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(TypeUrl);
      }
      if (ResponseNonce.Length != 0) {
        output.WriteRawTag(42);
        output.WriteString(ResponseNonce);
      }
      if (errorDetail_ != null) {
        output.WriteRawTag(50);
        output.WriteMessage(ErrorDetail);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (VersionInfo.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(VersionInfo);
      }
      if (node_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Node);
      }
      size += resourceNames_.CalculateSize(_repeated_resourceNames_codec);
      if (TypeUrl.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(TypeUrl);
      }
      if (ResponseNonce.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ResponseNonce);
      }
      if (errorDetail_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(ErrorDetail);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(DiscoveryRequest other) {
      if (other == null) {
        return;
      }
      if (other.VersionInfo.Length != 0) {
        VersionInfo = other.VersionInfo;
      }
      if (other.node_ != null) {
        if (node_ == null) {
          node_ = new global::Envoy.Api.V2.Core.Node();
        }
        Node.MergeFrom(other.Node);
      }
      resourceNames_.Add(other.resourceNames_);
      if (other.TypeUrl.Length != 0) {
        TypeUrl = other.TypeUrl;
      }
      if (other.ResponseNonce.Length != 0) {
        ResponseNonce = other.ResponseNonce;
      }
      if (other.errorDetail_ != null) {
        if (errorDetail_ == null) {
          errorDetail_ = new global::Google.Rpc.Status();
        }
        ErrorDetail.MergeFrom(other.ErrorDetail);
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
            VersionInfo = input.ReadString();
            break;
          }
          case 18: {
            if (node_ == null) {
              node_ = new global::Envoy.Api.V2.Core.Node();
            }
            input.ReadMessage(node_);
            break;
          }
          case 26: {
            resourceNames_.AddEntriesFrom(input, _repeated_resourceNames_codec);
            break;
          }
          case 34: {
            TypeUrl = input.ReadString();
            break;
          }
          case 42: {
            ResponseNonce = input.ReadString();
            break;
          }
          case 50: {
            if (errorDetail_ == null) {
              errorDetail_ = new global::Google.Rpc.Status();
            }
            input.ReadMessage(errorDetail_);
            break;
          }
        }
      }
    }

  }

  public sealed partial class DiscoveryResponse : pb::IMessage<DiscoveryResponse> {
    private static readonly pb::MessageParser<DiscoveryResponse> _parser = new pb::MessageParser<DiscoveryResponse>(() => new DiscoveryResponse());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<DiscoveryResponse> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Envoy.Api.V2.DiscoveryReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DiscoveryResponse() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DiscoveryResponse(DiscoveryResponse other) : this() {
      versionInfo_ = other.versionInfo_;
      resources_ = other.resources_.Clone();
      canary_ = other.canary_;
      typeUrl_ = other.typeUrl_;
      nonce_ = other.nonce_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DiscoveryResponse Clone() {
      return new DiscoveryResponse(this);
    }

    /// <summary>Field number for the "version_info" field.</summary>
    public const int VersionInfoFieldNumber = 1;
    private string versionInfo_ = "";
    /// <summary>
    /// The version of the response data.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string VersionInfo {
      get { return versionInfo_; }
      set {
        versionInfo_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "resources" field.</summary>
    public const int ResourcesFieldNumber = 2;
    private static readonly pb::FieldCodec<global::Google.Protobuf.WellKnownTypes.Any> _repeated_resources_codec
        = pb::FieldCodec.ForMessage(18, global::Google.Protobuf.WellKnownTypes.Any.Parser);
    private readonly pbc::RepeatedField<global::Google.Protobuf.WellKnownTypes.Any> resources_ = new pbc::RepeatedField<global::Google.Protobuf.WellKnownTypes.Any>();
    /// <summary>
    /// The response resources. These resources are typed and depend on the API being called.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Google.Protobuf.WellKnownTypes.Any> Resources {
      get { return resources_; }
    }

    /// <summary>Field number for the "canary" field.</summary>
    public const int CanaryFieldNumber = 3;
    private bool canary_;
    /// <summary>
    /// [#not-implemented-hide:]
    /// Canary is used to support two Envoy command line flags:
    ///
    /// * --terminate-on-canary-transition-failure. When set, Envoy is able to
    ///   terminate if it detects that configuration is stuck at canary. Consider
    ///   this example sequence of updates:
    ///   - Management server applies a canary config successfully.
    ///   - Management server rolls back to a production config.
    ///   - Envoy rejects the new production config.
    ///   Since there is no sensible way to continue receiving configuration
    ///   updates, Envoy will then terminate and apply production config from a
    ///   clean slate.
    /// * --dry-run-canary. When set, a canary response will never be applied, only
    ///   validated via a dry run.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Canary {
      get { return canary_; }
      set {
        canary_ = value;
      }
    }

    /// <summary>Field number for the "type_url" field.</summary>
    public const int TypeUrlFieldNumber = 4;
    private string typeUrl_ = "";
    /// <summary>
    /// Type URL for resources. This must be consistent with the type_url in the
    /// Any messages for resources if resources is non-empty. This effectively
    /// identifies the xDS API when muxing over ADS.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string TypeUrl {
      get { return typeUrl_; }
      set {
        typeUrl_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "nonce" field.</summary>
    public const int NonceFieldNumber = 5;
    private string nonce_ = "";
    /// <summary>
    /// For gRPC based subscriptions, the nonce provides a way to explicitly ack a
    /// specific DiscoveryResponse in a following DiscoveryRequest. Additional
    /// messages may have been sent by Envoy to the management server for the
    /// previous version on the stream prior to this DiscoveryResponse, that were
    /// unprocessed at response send time. The nonce allows the management server
    /// to ignore any further DiscoveryRequests for the previous version until a
    /// DiscoveryRequest bearing the nonce. The nonce is optional and is not
    /// required for non-stream based xDS implementations.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Nonce {
      get { return nonce_; }
      set {
        nonce_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as DiscoveryResponse);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(DiscoveryResponse other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (VersionInfo != other.VersionInfo) return false;
      if(!resources_.Equals(other.resources_)) return false;
      if (Canary != other.Canary) return false;
      if (TypeUrl != other.TypeUrl) return false;
      if (Nonce != other.Nonce) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (VersionInfo.Length != 0) hash ^= VersionInfo.GetHashCode();
      hash ^= resources_.GetHashCode();
      if (Canary != false) hash ^= Canary.GetHashCode();
      if (TypeUrl.Length != 0) hash ^= TypeUrl.GetHashCode();
      if (Nonce.Length != 0) hash ^= Nonce.GetHashCode();
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
      if (VersionInfo.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(VersionInfo);
      }
      resources_.WriteTo(output, _repeated_resources_codec);
      if (Canary != false) {
        output.WriteRawTag(24);
        output.WriteBool(Canary);
      }
      if (TypeUrl.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(TypeUrl);
      }
      if (Nonce.Length != 0) {
        output.WriteRawTag(42);
        output.WriteString(Nonce);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (VersionInfo.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(VersionInfo);
      }
      size += resources_.CalculateSize(_repeated_resources_codec);
      if (Canary != false) {
        size += 1 + 1;
      }
      if (TypeUrl.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(TypeUrl);
      }
      if (Nonce.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Nonce);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(DiscoveryResponse other) {
      if (other == null) {
        return;
      }
      if (other.VersionInfo.Length != 0) {
        VersionInfo = other.VersionInfo;
      }
      resources_.Add(other.resources_);
      if (other.Canary != false) {
        Canary = other.Canary;
      }
      if (other.TypeUrl.Length != 0) {
        TypeUrl = other.TypeUrl;
      }
      if (other.Nonce.Length != 0) {
        Nonce = other.Nonce;
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
            VersionInfo = input.ReadString();
            break;
          }
          case 18: {
            resources_.AddEntriesFrom(input, _repeated_resources_codec);
            break;
          }
          case 24: {
            Canary = input.ReadBool();
            break;
          }
          case 34: {
            TypeUrl = input.ReadString();
            break;
          }
          case 42: {
            Nonce = input.ReadString();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
