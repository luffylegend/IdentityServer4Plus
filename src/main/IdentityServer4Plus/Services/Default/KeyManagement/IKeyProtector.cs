// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;

namespace IdentityServer4.Services.KeyManagement;

/// <summary>
/// Interface to model protecting/unprotecting RsaKeyContainer.
/// </summary>
public interface ISigningKeyProtector
{
    /// <summary>
    /// Protects KeyContainer.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    SerializedKey Protect(KeyContainer key);

    /// <summary>
    /// Unprotects KeyContainer.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    KeyContainer Unprotect(SerializedKey key);
}
