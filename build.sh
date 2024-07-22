#!/usr/bin/env bash
set -euo pipefail

rm -rf nuget
mkdir nuget

dotnet tool restore

pushd ./src/IdentityServer4Plus.Storage
./build.sh "$@"
popd

pushd ./src/IdentityServer4Plus
./build.sh "$@"
popd

pushd ./src/IdentityServer4Plus.EntityFramework.Storage
./build.sh "$@"
popd

pushd ./src/IdentityServer4Plus.EntityFramework
./build.sh "$@"
popd

pushd ./src/IdentityServer4Plus.AspNetIdentity
./build.sh "$@"
popd
