// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


#nullable enable

using IdentityServer4.Configuration;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace IdentityServer4.Hosting.DynamicProviders;


/// <summary>
/// Context for configuring an authentication handler from a dynamic identity provider.
/// </summary>
/// <typeparam name="TAuthenticationOptions"></typeparam>
/// <typeparam name="TIdentityProvider"></typeparam>
public class ConfigureAuthenticationContext<TAuthenticationOptions, TIdentityProvider>
    where TAuthenticationOptions : AuthenticationSchemeOptions
    where TIdentityProvider : IdentityProvider
{
    /// <summary>
    /// The authentication options.
    /// </summary>
    public TAuthenticationOptions AuthenticationOptions { get; set; } = default!;

    /// <summary>
    /// The identity provider.
    /// </summary>
    public TIdentityProvider IdentityProvider { get; set; } = default!;

    /// <summary>
    /// The dynamic identity provider options.
    /// </summary>
    public DynamicProviderOptions DynamicProviderOptions { get; set; } = default!;

    /// <summary>
    /// The path prefix for callback paths the authentication handler is to use.
    /// </summary>
    public PathString PathPrefix { get; set; } = default!;
}
