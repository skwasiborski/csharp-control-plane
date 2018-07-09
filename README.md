# csharp-control-plane
c# implementation of an Envoy gRPC control plane

This repository contains a c#-based implementation of an API server that implements the discovery service APIs defined
in [data-plane-api](https://github.com/envoyproxy/data-plane-api). Functionaly it is a port of
[go-control-plane](https://github.com/envoyproxy/go-control-plane), but building an idiomatic c# implementation is
prioritized over exact interface parity with the Go implementation.
