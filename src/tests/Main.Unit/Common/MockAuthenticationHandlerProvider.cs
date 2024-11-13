// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace UnitTests.Common;

internal class MockAuthenticationHandlerProvider : IAuthenticationHandlerProvider
{
    public IAuthenticationHandler Handler { get; set; }

    public Task<IAuthenticationHandler> GetHandlerAsync(HttpContext context, string authenticationScheme)
    {
        return Task.FromResult(Handler);
    }
}
