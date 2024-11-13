// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


#nullable enable

using IdentityServer4.EntityFramework.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer4.EntityFramework;

/// <summary>
/// Interface to model notifications from the TokenCleanup feature.
/// </summary>
public interface IOperationalStoreNotification
{
    /// <summary>
    /// Notification for persisted grants being removed.
    /// </summary>
    /// <param name="persistedGrants"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task PersistedGrantsRemovedAsync(IEnumerable<PersistedGrant> persistedGrants, CancellationToken cancellationToken = default);

    /// <summary>
    /// Notification for device codes being removed.
    /// </summary>
    /// <param name="deviceCodes"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeviceCodesRemovedAsync(IEnumerable<DeviceFlowCodes> deviceCodes, CancellationToken cancellationToken = default);
}
