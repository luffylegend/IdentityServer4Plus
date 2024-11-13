// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;

namespace IdentityServer4.Extensions;

/// <summary>
/// Extensions for Key Management
/// </summary>
public static class KeyManagementExtensions
{
    internal static RsaSecurityKey CreateRsaSecurityKey(this KeyManagementOptions options)
    {
        return CryptoHelper.CreateRsaSecurityKey(options.RsaKeySize);
    }

    internal static bool IsRetired(this KeyManagementOptions options, TimeSpan age)
    {
        return (age >= options.KeyRetirementAge);
    }

    internal static bool IsExpired(this KeyManagementOptions options, TimeSpan age)
    {
        return (age >= options.RotationInterval);
    }

    internal static bool IsWithinInitializationDuration(this KeyManagementOptions options, TimeSpan age)
    {
        return (age <= options.InitializationDuration);
    }

    internal static TimeSpan GetAge(this IClock clock, DateTime date)
    {
        var now = clock.UtcNow.UtcDateTime;
        if (date > now) now = date;
        return now.Subtract(date);
    }
}
