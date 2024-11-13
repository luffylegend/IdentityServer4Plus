// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityModel.Client;
using System.Threading.Tasks;

namespace IdentityServer4.AccessToken;

/// <inheritdoc />
public class DefaultClientAssertionService : IClientAssertionService
{
    /// <inheritdoc />
    public Task<ClientAssertion?> GetClientAssertionAsync(string? clientName = null, TokenRequestParameters? parameters = null)
    {
        return Task.FromResult<ClientAssertion?>(null);
    }
}
