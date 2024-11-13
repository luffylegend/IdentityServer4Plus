// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using System;

namespace UnitTests.Common;

class MockSystemClock : IClock
{
    public DateTimeOffset Now { get; set; }

    public DateTimeOffset UtcNow
    {
        get
        {
            return Now;
        }
    }
}
