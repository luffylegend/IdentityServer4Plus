// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using IdentityServer4.Stores;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Hosting.DynamicProviders;

class NopIdentityProviderStore : IIdentityProviderStore
{
    public Task<IEnumerable<IdentityProviderName>> GetAllSchemeNamesAsync()
    {
        return Task.FromResult(Enumerable.Empty<IdentityProviderName>());
    }

    public Task<IdentityProvider> GetBySchemeAsync(string scheme)
    {
        return Task.FromResult<IdentityProvider>(null);
    }
}
