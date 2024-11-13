// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Extensions;
using System;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

#pragma warning disable 1591

namespace IdentityServer4.Stores.Serialization;

public class ClaimsPrincipalConverter : JsonConverter<ClaimsPrincipal>
{
    public override ClaimsPrincipal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var source = JsonSerializer.Deserialize<ClaimsPrincipalLite>(ref reader, options);
        return source?.ToClaimsPrincipal();
    }

    public override void Write(Utf8JsonWriter writer, ClaimsPrincipal value, JsonSerializerOptions options)
    {
        var target = value.ToClaimsPrincipalLite();
        JsonSerializer.Serialize(writer, target, options);
    }
}
