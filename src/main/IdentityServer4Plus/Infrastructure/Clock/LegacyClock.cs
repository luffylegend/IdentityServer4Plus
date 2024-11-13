// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Authentication;
using System;

namespace IdentityServer4;

class LegacyClock : IClock
{
#pragma warning disable CS0618 // Type or member is obsolete
    private readonly ISystemClock _clock;
#pragma warning restore CS0618 // Type or member is obsolete

#pragma warning disable CS0618 // Type or member is obsolete
    public LegacyClock(ISystemClock clock)
#pragma warning restore CS0618 // Type or member is obsolete
    {
        _clock = clock;
    }

    public DateTimeOffset UtcNow
    {
        get => _clock.UtcNow;
    }
}
