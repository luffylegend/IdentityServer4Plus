// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityModel.Client;

namespace IdentityServer4.AccessToken.Tests;

public class TestClientAssertionService : IClientAssertionService
{
    private readonly string _clientName;
    private readonly string _assertionType;
    private readonly string _assertionValue;

    public TestClientAssertionService(string clientName, string assertionType, string assertionValue)
    {
        _clientName = clientName;
        _assertionType = assertionType;
        _assertionValue = assertionValue;
    }

    public Task<ClientAssertion?> GetClientAssertionAsync(string? clientName = null, TokenRequestParameters? parameters = null)
    {
        if (clientName == _clientName)
        {
            return Task.FromResult<ClientAssertion?>(new()
            {
                Type = _assertionType,
                Value = _assertionValue
            });
        }

        return Task.FromResult<ClientAssertion?>(null);
    }
}
