$ErrorActionPreference = "Stop";

New-Item -ItemType Directory -Force -Path ./nuget

dotnet tool restore

pushd ./src/IdentityServer4Plus.Storage
Invoke-Expression "./build.ps1 $args"
popd

pushd ./src/IdentityServer4Plus
Invoke-Expression "./build.ps1 $args"
popd

pushd ./src/IdentityServer4Plus.EntityFramework.Storage
Invoke-Expression "./build.ps1 $args"
popd

pushd ./src/IdentityServer4Plus.EntityFramework
Invoke-Expression "./build.ps1 $args"
popd

pushd ./src/IdentityServer4Plus.AspNetIdentity
Invoke-Expression "./build.ps1 $args"
popd
