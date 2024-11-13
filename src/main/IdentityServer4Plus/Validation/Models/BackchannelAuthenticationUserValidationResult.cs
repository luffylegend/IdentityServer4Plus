// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


#nullable enable

using System;
using System.Security.Claims;

namespace IdentityServer4.Validation;

/// <summary>
/// Represents the result of a backchannel authentication request.
/// </summary>
public class BackchannelAuthenticationUserValidationResult
{
    /// <summary>
    /// Indicates if this represents an error.
    /// </summary>
    public bool IsError => !String.IsNullOrWhiteSpace(Error);

    /// <summary>
    /// Gets or sets the error.
    /// </summary>
    public string? Error { get; set; }

    /// <summary>
    /// Gets or sets the error description.
    /// </summary>
    public string? ErrorDescription { get; set; }

    /// <summary>
    /// Gets or sets the subject based upon the provided hint.
    /// </summary>
    public ClaimsPrincipal? Subject { get; set; }
}
