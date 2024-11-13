// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Threading.Tasks;

namespace IdentityServer4.Stores.Empty;


internal class EmptyClientStore : IClientStore
{
    public Task<Client> FindClientByIdAsync(string clientId)
    {
        return Task.FromResult<Client>(null);
    }
}
