// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Stores.Serialization;
using IdentityModel;
using System.Linq;
using System.Security.Claims;

namespace IdentityServer4.Extensions;

/// <summary>
///  Extension methods for ClaimsPrincipalLite
/// </summary>
public static class ClaimsPrincipalLiteExtensions
{
    /// <summary>
    /// Converts a ClaimsPrincipalLite to ClaimsPrincipal
    /// </summary>
    public static ClaimsPrincipal ToClaimsPrincipal(this ClaimsPrincipalLite principal)
    {
        var claims = principal.Claims.Select(x => new Claim(x.Type, x.Value, x.ValueType ?? ClaimValueTypes.String)).ToArray();
        var id = new ClaimsIdentity(claims, principal.AuthenticationType, JwtClaimTypes.Name, JwtClaimTypes.Role);

        return new ClaimsPrincipal(id);
    }

    /// <summary>
    /// Converts a ClaimsPrincipal to ClaimsPrincipalLite
    /// </summary>
    public static ClaimsPrincipalLite ToClaimsPrincipalLite(this ClaimsPrincipal principal)
    {
        var claims = principal.Claims.Select(
                x => new ClaimLite
                {
                    Type = x.Type,
                    Value = x.Value,
                    ValueType = x.ValueType == ClaimValueTypes.String ? null : x.ValueType
                }).ToArray();

        return new ClaimsPrincipalLite
        {
            AuthenticationType = principal.Identity!.AuthenticationType!,
            Claims = claims
        };
    }
}
