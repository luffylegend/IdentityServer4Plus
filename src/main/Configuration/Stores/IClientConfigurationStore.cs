// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;

namespace IdentityServer4.Configuration;

/// <summary>
/// Interface for communication with the client configuration data store.
/// </summary>
public interface IClientConfigurationStore
{
    /// <summary>
    /// Adds a client to the configuration store.
    /// </summary>
    /// <param name="client">The client to add to the store</param>
    Task AddAsync(Client client);
}
