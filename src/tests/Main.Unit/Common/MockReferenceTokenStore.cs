// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using IdentityServer4.Stores;
using System;
using System.Threading.Tasks;

namespace UnitTests.Common;

class MockReferenceTokenStore : IReferenceTokenStore
{
    public Task<Token> GetReferenceTokenAsync(string handle)
    {
        throw new NotImplementedException();
    }

    public Task RemoveReferenceTokenAsync(string handle)
    {
        throw new NotImplementedException();
    }

    public Task RemoveReferenceTokensAsync(string subjectId, string clientId, string sessionId = null)
    {
        throw new NotImplementedException();
    }

    public Task<string> StoreReferenceTokenAsync(Token token)
    {
        throw new NotImplementedException();
    }
}
