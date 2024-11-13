# IdentityServer4.Configuration

## Overview
IdentityServer4.Configuration provides a collection of endpoints that allow for management and configuration of an IdentityServer implementation using the Dynamic Client Registration [protocol](https://datatracker.ietf.org/doc/html/rfc7591). The Configuration API can be hosted either separately or within an IdentityServer implementation. 

This package includes abstractions for interacting with the IdentityServer configuration data store. You can either implement the store yourself or use [IdentityServer4.Configuration.EntityFramework](https://www.nuget.org/packages/IdentityServer4.Configuration.EntityFramework) for our default store implementation built with Entity Framework.

## Reporting Issues and Getting Support
- For bug reports or feature requests, open an issue on GitHub: [Submit an Issue](https://github.com/luffylegend/IdentityServer4Plus/issues).

## Related Packages
- [IdentityServer4Plus](https://www.nuget.org/packages/IdentityServer4Plus): OAuth and OpenID Connect framework with in-memory or customizable persistence.
- [IdentityServer4Plus.EntityFramework](https://www.nuget.org/packages/IdentityServer4Plus.EntityFramework.Storage): OAuth and OpenId Connect framework with Entity Framework based persistence.
- [IdentityServer4Plus.AspNetIdentity](https://www.nuget.org/packages/IdentityServer4Plus.AspNetIdentity): Integration between ASP.NET Core Identity and IdentityServer.
- [IdentityServer4Plus.EntityFramework.Configuration](https://www.nuget.org/packages/IdentityServer4Plus.Configuration.EntityFramework): Configuration API for IdentityServer with Entity Framework based persistence.
- [IdentityServer4Plus.Storage](https://www.nuget.org/packages/IdentityServer4Plus.Storage): Support package containing models and interfaces for the persistence layer of IdentityServer.
- [IdentityServer4Plus.EntityFramework.Storage](https://www.nuget.org/packages/IdentityServer4Plus.EntityFramework.Storage): Support package containing an implementation of the persistence layer of IdentityServer implemented with Entity Framework.
