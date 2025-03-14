// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

#pragma warning disable 1591

namespace IdentityServer4.Stores.Serialization
{
    public class ClaimsPrincipalConverter : JsonConverter<ClaimsPrincipal>
    {
        public override ClaimsPrincipal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Deserialize the JSON into ClaimsPrincipalLite
            var source = JsonSerializer.Deserialize<ClaimsPrincipalLite>(ref reader, options);
            if (source == null) return null;

            // Convert ClaimsPrincipalLite to ClaimsPrincipal
            var claims = source.Claims.Select(x => new Claim(x.Type, x.Value, x.ValueType));
            var identity = new ClaimsIdentity(claims, source.AuthenticationType, JwtClaimTypes.Name, JwtClaimTypes.Role);
            return new ClaimsPrincipal(identity);
        }

        public override void Write(Utf8JsonWriter writer, ClaimsPrincipal value, JsonSerializerOptions options)
        {
            // Convert ClaimsPrincipal to ClaimsPrincipalLite
            var target = new ClaimsPrincipalLite
            {
                AuthenticationType = value.Identity.AuthenticationType,
                Claims = value.Claims.Select(x => new ClaimLite { Type = x.Type, Value = x.Value, ValueType = x.ValueType }).ToArray()
            };

            // Serialize ClaimsPrincipalLite to JSON
            JsonSerializer.Serialize(writer, target, options);
        }
    }
}
