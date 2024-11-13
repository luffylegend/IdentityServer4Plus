// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace UnitTests.Common;

internal class StubSessionCoordinationService : ISessionCoordinationService
{
    public Task ProcessExpirationAsync(UserSession session)
    {
        return Task.CompletedTask;
    }

    public Task ProcessLogoutAsync(UserSession session)
    {
        return Task.CompletedTask;
    }

    public Task<bool> ValidateSessionAsync(SessionValidationRequest request)
    {
        return Task.FromResult(true);
    }
}
