// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


#nullable enable

using System.Threading.Tasks;

namespace IdentityServer4.Validation;

/// <summary>
/// Interface for the backchannel authentication user validation
/// </summary>
public interface IBackchannelAuthenticationUserValidator
{
    /// <summary>
    /// Validates the user.
    /// </summary>
    /// <param name="userValidatorContext"></param>
    /// <returns></returns>
    Task<BackchannelAuthenticationUserValidationResult> ValidateRequestAsync(BackchannelAuthenticationUserValidatorContext userValidatorContext);
}
