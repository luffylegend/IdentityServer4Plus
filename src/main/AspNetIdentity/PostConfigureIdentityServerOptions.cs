// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace IdentityServer4.AspNetIdentity;

/// <summary>
/// Identity server options configuration
/// </summary>
public class UseAspNetIdentityCookieScheme : IPostConfigureOptions<IdentityServerOptions>
{
    private readonly IOptions<Microsoft.AspNetCore.Authentication.AuthenticationOptions> _authOptions;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="authOptions"></param>
    public UseAspNetIdentityCookieScheme(IOptions<Microsoft.AspNetCore.Authentication.AuthenticationOptions> authOptions)
    {
        _authOptions = authOptions;
    }

    /// <inheritdoc/>
    public void PostConfigure(string name, IdentityServerOptions options)
    {
        // If we are using ASP.NET Identity and the dynamic providers don't have a
        // sign out scheme set, then we need the dynamic providers to use ASP.NET
        // Identity's cookie at sign out time. If the sign out scheme is explicitly
        // set, then we don't override that though.

        if (DefaultAuthSchemeIsAspNetIdentity() &&
            !options.DynamicProviders.SignOutSchemeSetExplicitly)
        {
            options.DynamicProviders.SignOutScheme = IdentityConstants.ApplicationScheme;
        }

        bool DefaultAuthSchemeIsAspNetIdentity() =>
            _authOptions.Value.DefaultAuthenticateScheme == IdentityConstants.ApplicationScheme;
    }
}
