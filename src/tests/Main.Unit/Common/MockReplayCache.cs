// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Services;
using System;
using System.Threading.Tasks;

namespace UnitTests.Common;

public class MockReplayCache : IReplayCache
{
    public bool Exists { get; set; }

    public Task AddAsync(string purpose, string handle, DateTimeOffset expiration)
    {
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string purpose, string handle)
    {
        return Task.FromResult(Exists);
    }
}
