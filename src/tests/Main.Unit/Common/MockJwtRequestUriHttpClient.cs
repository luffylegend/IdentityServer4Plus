// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Threading.Tasks;

namespace UnitTests.Common;

public class MockJwtRequestUriHttpClient : IJwtRequestUriHttpClient
{
    public string Jwt { get; set; }

    public Task<string> GetJwtAsync(string url, Client client)
    {
        return Task.FromResult(Jwt);
    }
}
