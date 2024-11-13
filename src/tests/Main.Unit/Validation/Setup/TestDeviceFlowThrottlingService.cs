// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Threading.Tasks;

namespace UnitTests.Validation.Setup;

public class TestDeviceFlowThrottlingService : IDeviceFlowThrottlingService
{
    private readonly bool _shouldSlownDown;

    public TestDeviceFlowThrottlingService(bool shouldSlownDown = false)
    {
        this._shouldSlownDown = shouldSlownDown;
    }

    public Task<bool> ShouldSlowDown(string deviceCode, DeviceCode details) => Task.FromResult(_shouldSlownDown);
}
