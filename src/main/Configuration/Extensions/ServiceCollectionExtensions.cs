// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer4.Configuration;


/// <summary>
/// Extension methods for adding the entity framework based client configuration
/// store implementation to DI.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the in memory client configuration store to DI. 
    /// </summary>
    /// <remark>
    /// This is for testing and demos only.
    /// </remark>
    public static IServiceCollection AddInMemoryClientConfigurationStore(this IdentityServerConfigurationBuilder builder)
    {
        return builder.Services.AddTransient<IClientConfigurationStore, InMemoryClientConfigurationStore>();
    }
}
