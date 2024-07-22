Remove-Item $env:USERPROFILE\.nuget\packages\IdentityServer4Plus.\ -Recurse -ErrorAction SilentlyContinue 
Remove-Item $env:USERPROFILE\.nuget\packages\IdentityServer4Plus.storage\ -Recurse -ErrorAction SilentlyContinue 
Remove-Item $env:USERPROFILE\.nuget\packages\IdentityServer4Plus.entityframework\ -Recurse -ErrorAction SilentlyContinue 
Remove-Item $env:USERPROFILE\.nuget\packages\IdentityServer4Plus.entityframework.storage\ -Recurse -ErrorAction SilentlyContinue 
Remove-Item $env:USERPROFILE\.nuget\packages\IdentityServer4Plus.aspnetidentity\ -Recurse -ErrorAction SilentlyContinue 

Remove-Item $env:USERPROFILE\.nuget\packages\identitymodel\ -Recurse -ErrorAction SilentlyContinue 
Remove-Item $env:USERPROFILE\.nuget\packages\IdentityModel.AspNetCore.OAuth2Introspection\ -Recurse -ErrorAction SilentlyContinue 
Remove-Item $env:USERPROFILE\.nuget\packages\IdentityServer4Plus.AccessTokenValidation\ -Recurse -ErrorAction SilentlyContinue 