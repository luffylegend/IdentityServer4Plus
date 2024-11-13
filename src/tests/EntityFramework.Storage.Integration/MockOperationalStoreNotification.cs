// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.EntityFramework;
using IdentityServer4.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EntityFramework.Storage.IntegrationTests;

public class MockOperationalStoreNotification : IOperationalStoreNotification
{
    public readonly List<IEnumerable<PersistedGrant>> PersistedGrantNotifications = new();
    public readonly List<IEnumerable<DeviceFlowCodes>> DeviceFlowCodeNotifications = new();

    public Action<IEnumerable<PersistedGrant>> OnPersistedGrantsRemoved = _ => { };
    public Action<IEnumerable<DeviceFlowCodes>> OnDeviceFlowCodesRemoved = _ => { };

    public Task PersistedGrantsRemovedAsync(IEnumerable<PersistedGrant> persistedGrants, CancellationToken cancellationToken = default)
    {
        OnPersistedGrantsRemoved(persistedGrants);
        PersistedGrantNotifications.Add(persistedGrants);
        return Task.CompletedTask;
    }

    public Task DeviceCodesRemovedAsync(IEnumerable<DeviceFlowCodes> deviceCodes, CancellationToken cancellationToken = default)
    {
        OnDeviceFlowCodesRemoved(deviceCodes);
        DeviceFlowCodeNotifications.Append(deviceCodes);
        return Task.CompletedTask;
    }

}
