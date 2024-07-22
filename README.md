## About IdentityServer4Plus
Due to the discontinuation of maintenance for ```Identity Server 4``` and my need for it to support a higher version of ```.net8.0```, I downloaded it, upgraded it, and resolved some issues and warnings.

So ```IdentityServer4Plus``` was born on this basis.

```IdentityServer4Plus``` is a personal maintenance project that aims to provide a certain degree of continuity for the purpose of learning and communication.

IdentityServer is a free, open source [OpenID Connect](http://openid.net/connect/) and [OAuth 2.0](https://tools.ietf.org/html/rfc6749) framework for ASP.NET Core.
```IdentityServer4Plus``` incorporates all the protocol implementations and extensibility points needed to integrate token-based authentication, single-sign-on and API access control in your applications.
```IdentityServer4Plus``` is officially [certified](https://openid.net/certification/) by the [OpenID Foundation](https://openid.net) and thus spec-compliant and interoperable.
It is part of the [.NET Foundation](https://www.dotnetfoundation.org/), and operates under their [code of conduct](https://www.dotnetfoundation.org/code-of-conduct). It is licensed under [Apache 2](https://opensource.org/licenses/Apache-2.0) (an OSI approved license).

For project documentation, please visit [readthedocs](https://identityserver4.readthedocs.io).

## Branch structure
Active development happens on the main branch. This always contains the latest version. Each (pre-) release is tagged with the corresponding version. 

The [aspnetcore1](https://github.com/luffylegend/IdentityServer4Plus/tree/aspnetcore1) and [aspnetcore2](https://github.com/luffylegend/IdentityServer4Plus/tree/aspnetcore2) branches contain the latest versions of the older ASP.NET Core based versions.

## How to build

* [Install](https://www.microsoft.com/net/download/core#/current) the latest .NET Core 8.0 SDK
* Install Git
* Clone this repo
* Run `build.ps1` or `build.sh` in the root of the cloned repo

## Documentation
For project documentation, please visit [readthedocs](https://identityserver4.readthedocs.io).

See [here](http://docs.identityserver.io/en/aspnetcore1/) for the 1.x docs, and [here](http://docs.identityserver.io/en/aspnetcore2/) for the 2.x docs.

## Bug reports and feature requests
Please use the [issue tracker](https://github.com/luffylegend/IdentityServer4Plus/issues) for that. We only support the latest version for free. For older versions, you can get a commercial support agreement with me.

## Acknowledgements
IdentityServer4 is built using the following great open source projects and free services:

* [IdentityServer4](https://github.com/identityserver/IdentityServer4)
* [ASP.NET Core](https://github.com/dotnet/aspnetcore)
* [Bullseye](https://github.com/adamralph/bullseye)
* [SimpleExec](https://github.com/adamralph/simple-exec)
* [MinVer](https://github.com/adamralph/minver)
* [Json.Net](http://www.newtonsoft.com/json)
* [XUnit](https://xunit.github.io/)
* [Fluent Assertions](http://www.fluentassertions.com/)
* [GitReleaseManager](https://github.com/GitTools/GitReleaseManager)

..and last but not least a big thanks to all our [contributors](https://github.com/luffylegend/IdentityServer4Plus/graphs/contributors)!
