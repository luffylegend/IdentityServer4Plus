// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Validation;
using System.Threading.Tasks;

namespace UnitTests.Validation.Setup;

internal class TestBackchannelAuthenticationRequestIdValidator : IBackchannelAuthenticationRequestIdValidator
{
    private readonly bool _shouldError;

    public TestBackchannelAuthenticationRequestIdValidator(bool shouldError = false)
    {
        this._shouldError = shouldError;
    }

    //public DeviceCode DeviceCodeResult { get; set; } = new DeviceCode();

    public Task ValidateAsync(BackchannelAuthenticationRequestIdValidationContext context)
    {
        if (_shouldError) context.Result = new TokenRequestValidationResult(context.Request, "error");
        else context.Result = new TokenRequestValidationResult(context.Request);

        //context.Request.DeviceCode = DeviceCodeResult;

        return Task.CompletedTask;
    }
}
