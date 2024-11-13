// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;

namespace IdentityServer4.Internal;

/// <summary>
/// Interface to model locking.
/// </summary>
public interface IConcurrencyLock<T>
{
    /// <summary>
    /// Locks. Returns false if lock was not obtained within in the timeout.
    /// </summary>
    /// <returns></returns>
    Task<bool> LockAsync(int millisecondsTimeout);

    /// <summary>
    /// Unlocks
    /// </summary>
    /// <returns></returns>
    void Unlock();
}
