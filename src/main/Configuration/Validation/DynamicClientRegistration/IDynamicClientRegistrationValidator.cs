// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Configuration.Models.DynamicClientRegistration;

namespace IdentityServer4.Configuration.Validation.DynamicClientRegistration;

/// <summary>
/// Validates a dynamic client registration request.
/// </summary>
public interface IDynamicClientRegistrationValidator
{
    /// <summary>
    /// Validates a dynamic client registration request. 
    /// </summary>
    /// <param name="context">Contextual information about the DCR
    /// request.</param>
    /// <returns>A task that returns an <see
    /// cref="IDynamicClientRegistrationValidationResult"/>, which either
    /// indicates success or failure.</returns>
    Task<IDynamicClientRegistrationValidationResult> ValidateAsync(DynamicClientRegistrationContext context);
}
