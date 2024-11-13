// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Http;

namespace UnitTests.Common;

/// <summary>
/// An implementation of IHttpContextAccessor that always returns null, to
/// simulate situations where there is no http context.
/// </summary>
internal class NullHttpContextAccessor : IHttpContextAccessor
{
    public HttpContext HttpContext
    {
        get
        {
            return null;
        }
        set
        {
            // Deliberate no-op
        }
    }
}
