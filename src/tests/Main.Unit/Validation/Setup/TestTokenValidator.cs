// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using IdentityServer4.Validation;
using System.Threading.Tasks;

namespace UnitTests.Validation.Setup;

class TestTokenValidator : ITokenValidator
{
    private readonly TokenValidationResult _result;

    public TestTokenValidator(TokenValidationResult result)
    {
        _result = result;
    }

    public Task<TokenValidationResult> ValidateAccessTokenAsync(string token, string expectedScope = null)
    {
        return Task.FromResult(_result);
    }

    public Task<TokenValidationResult> ValidateIdentityTokenAsync(string token, string clientId = null, bool validateLifetime = true)
    {
        return Task.FromResult(_result);
    }

    public Task<TokenValidationResult> ValidateRefreshTokenAsync(string token, Client client = null)
    {
        return Task.FromResult(_result);
    }
}
