#!/usr/bin/env bash

PROTOC=~/.nuget/packages/grpc.tools/1.13.0/tools/macosx_x64/protoc
GRPC=~/.nuget/packages/grpc.tools/1.13.0/tools/macosx_x64/grpc_csharp_plugin

for f in $(find protos -type f -name *.proto); 
do 
    if [ $(dirname "$f") != "protos/google/protobuf" ] 
    then 
        mkdir -p ./pb/$(dirname ${f#protos/});
        "$PROTOC" -Iprotos --csharp_out ./pb/$(dirname ${f#protos/}) --csharp_opt=file_extension=.g.cs --grpc_out ./pb/$(dirname ${f#protos/}) --plugin=protoc-gen-grpc="$GRPC" "$f";
    fi
done