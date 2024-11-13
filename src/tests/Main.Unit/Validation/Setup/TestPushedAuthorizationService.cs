// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTests.Validation.Setup;

/// <summary>
/// Test implementation of the pushed authorization service. Always returns a setup
/// pushed authorization request.
/// </summary>
internal class TestPushedAuthorizationService : IPushedAuthorizationService
{
    Dictionary<string, DeserializedPushedAuthorizationRequest> _pushedRequests = new();


    public Task ConsumeAsync(string referenceValue)
    {
        _pushedRequests.Remove(referenceValue);
        return Task.CompletedTask;
    }

    public Task<DeserializedPushedAuthorizationRequest> GetPushedAuthorizationRequestAsync(string referenceValue)
    {
        _pushedRequests.TryGetValue(referenceValue, out var par);
        return Task.FromResult(par);
    }

    public Task StoreAsync(DeserializedPushedAuthorizationRequest request)
    {
        _pushedRequests[request.ReferenceValue] = request;
        return Task.CompletedTask;
    }
}
