// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Http;
using System.Threading;

namespace IdentityServer4.Services.Default;

/// <summary>
/// Provides cancellation tokens based on the incoming http request
/// </summary>
class DefaultCancellationTokenProvider : ICancellationTokenProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="httpContextAccessor"></param>
    public DefaultCancellationTokenProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Provides access to the cancellation token from the http context
    /// </summary>
    public CancellationToken CancellationToken => _httpContextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;
}
