// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


#nullable enable

using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer4.EntityFramework;

/// <summary>
/// Service that cleans up persisted grants and device codes that are no longer
/// needed.
/// </summary>
public interface ITokenCleanupService
{
    /// <summary>
    /// Removes expired persisted grants, expired device codes, and optionally
    /// consumed persisted grants from the stores.
    /// </summary>
    /// <param name="cancellationToken">A token that propagates notification
    /// that the cleanup operation should be canceled.</param>
    /// <returns></returns>
    Task CleanupGrantsAsync(CancellationToken cancellationToken = default);
}
