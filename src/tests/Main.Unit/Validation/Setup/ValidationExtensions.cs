// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace UnitTests.Validation.Setup;

public static class ValidationExtensions
{
    public static ClientSecretValidationResult ToValidationResult(this Client client, ParsedSecret secret = null)
    {
        return new ClientSecretValidationResult
        {
            Client = client,
            Secret = secret
        };
    }
}
