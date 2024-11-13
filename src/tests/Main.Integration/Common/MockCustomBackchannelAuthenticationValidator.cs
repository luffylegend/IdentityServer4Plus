// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Validation;
using System;
using System.Threading.Tasks;

namespace IntegrationTests.Common;

internal class MockCustomBackchannelAuthenticationValidator : ICustomBackchannelAuthenticationValidator
{
    public CustomBackchannelAuthenticationRequestValidationContext Context { get; set; }


    /// <summary>
    /// An action that will be performed by the mock custom validator.
    /// </summary>
    public Action<CustomBackchannelAuthenticationRequestValidationContext> Thunk { get; set; } = delegate { };

    public Task ValidateAsync(CustomBackchannelAuthenticationRequestValidationContext customValidationContext)
    {
        Thunk(customValidationContext);
        Context = customValidationContext;
        return Task.CompletedTask;
    }
}
