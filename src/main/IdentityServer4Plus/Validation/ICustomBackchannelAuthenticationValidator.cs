// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;

namespace IdentityServer4.Validation;


/// <summary>
/// Extensibility point for CIBA authentication request validation.
/// </summary>
public interface ICustomBackchannelAuthenticationValidator
{
    /// <summary>
    /// Validates a CIBA authentication request.
    /// </summary>
    /// <param name="customValidationContext"></param>
    /// <returns></returns>
    Task ValidateAsync(CustomBackchannelAuthenticationRequestValidationContext customValidationContext);
}
