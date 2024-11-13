// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


#nullable enable

using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer4.Validation;

/// <summary>
/// Models the result of JWT request validation.
/// </summary>
public class JwtRequestValidationResult : ValidationResult
{
    /// <summary>
    /// The key/value pairs from the JWT payload of a successfully validated
    /// request, or null if a validation error occurred.
    /// </summary>
    public IEnumerable<Claim>? Payload { get; set; }
}
