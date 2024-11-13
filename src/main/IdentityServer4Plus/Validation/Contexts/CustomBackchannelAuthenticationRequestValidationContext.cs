// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace IdentityServer4.Validation;

/// <summary>
/// The validation context for a custom CIBA validator.
/// </summary>
public class CustomBackchannelAuthenticationRequestValidationContext
{
    /// <summary>
    /// Creates a new instance of the <see cref="CustomBackchannelAuthenticationRequestValidationContext"/> 
    /// </summary>
    public CustomBackchannelAuthenticationRequestValidationContext(BackchannelAuthenticationRequestValidationResult validatedRequest)
    {
        ValidationResult = validatedRequest;
    }
    /// <summary>
    /// Gets or sets the CIBA validation result.
    /// </summary>
    public BackchannelAuthenticationRequestValidationResult ValidationResult { get; set; }
}
