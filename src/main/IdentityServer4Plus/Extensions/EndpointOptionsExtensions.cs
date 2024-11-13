// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Configuration;
using IdentityServer4.Hosting;

namespace IdentityServer4.Extensions;

internal static class EndpointOptionsExtensions
{
    public static bool IsEndpointEnabled(this EndpointsOptions options, Endpoint endpoint)
    {
        return endpoint?.Name switch
        {
            IdentityServerConstants.EndpointNames.Authorize => options.EnableAuthorizeEndpoint,
            IdentityServerConstants.EndpointNames.CheckSession => options.EnableCheckSessionEndpoint,
            IdentityServerConstants.EndpointNames.DeviceAuthorization => options.EnableDeviceAuthorizationEndpoint,
            IdentityServerConstants.EndpointNames.Discovery => options.EnableDiscoveryEndpoint,
            IdentityServerConstants.EndpointNames.EndSession => options.EnableEndSessionEndpoint,
            IdentityServerConstants.EndpointNames.Introspection => options.EnableIntrospectionEndpoint,
            IdentityServerConstants.EndpointNames.Revocation => options.EnableTokenRevocationEndpoint,
            IdentityServerConstants.EndpointNames.Token => options.EnableTokenEndpoint,
            IdentityServerConstants.EndpointNames.UserInfo => options.EnableUserInfoEndpoint,
            IdentityServerConstants.EndpointNames.PushedAuthorization => options.EnablePushedAuthorizationEndpoint,
            _ => true
        };
    }
}
