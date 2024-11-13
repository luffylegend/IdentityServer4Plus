// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Text.Json;

namespace IdentityServer4.EntityFramework.Mappers;

internal static class PropertiesConverter
{
    public static string Convert(Dictionary<string, string> sourceMember)
    {
        return JsonSerializer.Serialize(sourceMember);
    }

    public static Dictionary<string, string> Convert(string sourceMember)
    {
        if (String.IsNullOrWhiteSpace(sourceMember))
        {
            return new Dictionary<string, string>();
        }
        return JsonSerializer.Deserialize<Dictionary<string, string>>(sourceMember);
    }
}
