// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace IdentityServer4.Configuration.EntityFramework;


/// <summary>
/// Extension methods for adding the entity framework based client configuration
/// store implementation to DI.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the entity framework based client configuration store
    /// implementation to DI.
    /// </summary>
    public static IServiceCollection AddClientConfigurationStore(this IdentityServerConfigurationBuilder builder)
    {
        builder.Services.TryAddTransient<ICancellationTokenProvider, DefaultCancellationTokenProvider>();
        builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        return builder.Services.AddTransient<IClientConfigurationStore, ClientConfigurationStore>();
    }
}
