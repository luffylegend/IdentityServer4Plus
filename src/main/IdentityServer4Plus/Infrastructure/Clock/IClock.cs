// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;

namespace IdentityServer4;

/// <summary>
/// Abstraction for the date/time.
/// </summary>
public interface IClock
{
    /// <summary>
    /// The current UTC date/time.
    /// </summary>
    DateTimeOffset UtcNow { get; }
}
