// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Threading.Tasks;

namespace UnitTests.Common;

class MockTokenCreationService : ITokenCreationService
{
    public string TokenResult { get; set; }
    public Token Token { get; set; }

    public Task<string> CreateTokenAsync(Token token)
    {
        Token = token;
        return Task.FromResult(TokenResult);
    }
}
