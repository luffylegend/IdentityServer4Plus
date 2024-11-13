// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Validation;
using System.Threading.Tasks;

namespace IntegrationTests.Common;

internal class MockCibaUserValidator : IBackchannelAuthenticationUserValidator
{
    public BackchannelAuthenticationUserValidationResult Result { get; set; } = new BackchannelAuthenticationUserValidationResult();
    public BackchannelAuthenticationUserValidatorContext UserValidatorContext { get; set; }

    public Task<BackchannelAuthenticationUserValidationResult> ValidateRequestAsync(BackchannelAuthenticationUserValidatorContext userValidatorContext)
    {
        UserValidatorContext = userValidatorContext;
        return Task.FromResult(Result);
    }
}
