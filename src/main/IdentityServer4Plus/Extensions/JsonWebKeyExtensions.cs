// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Text.Json;
using IdentityModel;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer4.Extensions;

/// <summary>
/// Extensions methods for JsonWebKey
/// </summary>
static class JsonWebKeyExtensions
{
    /// <summary>
    /// Create the value of a thumbprint-based cnf claim
    /// </summary>
    public static string CreateThumbprintCnf(this JsonWebKey jwk)
    {
        var jkt = jwk.CreateThumbprint();
        var values = new Dictionary<string, string>
    {
        { JwtClaimTypes.ConfirmationMethods.JwkThumbprint, jkt }
    };
        return JsonSerializer.Serialize(values);
    }
    /// <summary>
    /// Create the value of a thumbprint
    /// </summary>
    public static string CreateThumbprint(this JsonWebKey jwk)
    {
        var jkt = Base64Url.Encode(jwk.ComputeJwkThumbprint());
        return jkt;
    }
}
