// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer4.Services.KeyManagement;

/// <summary>
/// Interface to model loading the keys.
/// </summary>
public interface IKeyManager
{
    /// <summary>
    /// Returns the current signing keys.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<KeyContainer>> GetCurrentKeysAsync();

    /// <summary>
    /// Returns all the validation keys.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<KeyContainer>> GetAllKeysAsync();
}
