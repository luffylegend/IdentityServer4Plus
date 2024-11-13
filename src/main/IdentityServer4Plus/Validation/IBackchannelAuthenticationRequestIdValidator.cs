// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;

namespace IdentityServer4.Validation;

/// <summary>
/// The backchannel authentication request id validator
/// </summary>
public interface IBackchannelAuthenticationRequestIdValidator
{
    /// <summary>
    /// Validates the authentication request id.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    Task ValidateAsync(BackchannelAuthenticationRequestIdValidationContext context);
}
