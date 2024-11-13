// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Threading.Tasks;

namespace IntegrationTests.Common;

internal class MockCibaUserNotificationService : IBackchannelAuthenticationUserNotificationService
{
    public BackchannelUserLoginRequest LoginRequest { get; set; }

    public Task SendLoginRequestAsync(BackchannelUserLoginRequest request)
    {
        LoginRequest = request;
        return Task.CompletedTask;
    }
}
