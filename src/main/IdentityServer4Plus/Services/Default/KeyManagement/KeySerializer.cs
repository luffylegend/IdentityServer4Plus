// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Text.Json;

namespace IdentityServer4.Services.KeyManagement;

internal static class KeySerializer
{
    static JsonSerializerOptions _settings =
        new JsonSerializerOptions
        {
            IncludeFields = true
        };

    public static string Serialize<T>(T item)
    {
        return JsonSerializer.Serialize(item, item.GetType(), _settings);
    }

    public static T Deserialize<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, _settings);
    }
}
