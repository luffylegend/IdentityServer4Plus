// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace IdentityServer4.AccessToken.Tests;

public class TestDPoPNonceStore : IDPoPNonceStore
{
    public Task<string?> GetNonceAsync(DPoPNonceContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<string?>(null);
    }

    public Task StoreNonceAsync(DPoPNonceContext context, string nonce, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
