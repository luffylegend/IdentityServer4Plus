// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer4.Internal;

/// <summary>
/// Default implementation.
/// </summary>
public class DefaultConcurrencyLock<T> : IConcurrencyLock<T>
{
    static readonly SemaphoreSlim Lock = new SemaphoreSlim(1);

    /// <inheritdoc/>
    public Task<bool> LockAsync(int millisecondsTimeout)
    {
        if (millisecondsTimeout <= 0)
        {
            throw new ArgumentException("millisecondsTimeout must be greater than zero.");
        }

        return Lock.WaitAsync(millisecondsTimeout);
    }

    /// <inheritdoc/>
    public void Unlock()
    {
        Lock.Release();
    }
}
