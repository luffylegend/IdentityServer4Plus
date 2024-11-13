// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Stores.Empty;

internal class EmptyResourceStore : IResourceStore
{
    public Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
    {
        return Task.FromResult(Enumerable.Empty<ApiResource>());
    }

    public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
    {
        return Task.FromResult(Enumerable.Empty<ApiResource>());
    }

    public Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
    {
        return Task.FromResult(Enumerable.Empty<ApiScope>());
    }

    public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
    {
        return Task.FromResult(Enumerable.Empty<IdentityResource>());
    }

    public Task<Resources> GetAllResourcesAsync()
    {
        return Task.FromResult(new Resources() { OfflineAccess = true });
    }
}
