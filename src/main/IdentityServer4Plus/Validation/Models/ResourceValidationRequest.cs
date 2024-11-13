// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


#nullable enable

using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer4.Validation;

/// <summary>
/// Models the request to validate scopes and resource indicators for a client.
/// </summary>
public class ResourceValidationRequest
{
    /// <summary>
    /// The client.
    /// </summary>
    public Client Client { get; set; } = default!;

    /// <summary>
    /// The requested scope values.
    /// </summary>
    public IEnumerable<string> Scopes { get; set; } = default!;

    /// <summary>
    /// The requested resource indicators.
    /// </summary>
    public IEnumerable<string>? ResourceIndicators { get; set; }
}
