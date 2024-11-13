// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Services;
using System.Threading.Tasks;

namespace UnitTests.Validation.Setup;

internal class TestIssuerNameService : IIssuerNameService
{
    private readonly string _value;

    public TestIssuerNameService(string value = null)
    {
        _value = value ?? "https://identityserver4";
    }

    public Task<string> GetCurrentAsync()
    {
        return Task.FromResult(_value);
    }
}
