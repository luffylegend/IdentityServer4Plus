// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer4.Services.KeyManagement;

/// <summary>
/// Nop implementation of ISigningKeyStoreCache that does not cache keys.
/// </summary>
class NopKeyStoreCache : ISigningKeyStoreCache
{
    /// <summary>
    /// Returns null.
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<KeyContainer>> GetKeysAsync()
    {
        return Task.FromResult<IEnumerable<KeyContainer>>(null);
    }

    /// <summary>
    /// Does not cache keys.
    /// </summary>
    /// <param name="keys"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public Task StoreKeysAsync(IEnumerable<KeyContainer> keys, TimeSpan duration)
    {
        return Task.CompletedTask;
    }
}
