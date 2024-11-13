// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Extensions;
using IdentityServer4.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer4.Endpoints.Results;

/// <summary>
/// The result of userinfo 
/// </summary>
public class UserInfoResult : EndpointResult<UserInfoResult>
{
    /// <summary>
    /// The claims
    /// </summary>
    public Dictionary<string, object> Claims { get; }

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="claims"></param>
    public UserInfoResult(Dictionary<string, object> claims)
    {
        Claims = claims ?? throw new ArgumentNullException(nameof(claims));
    }
}

internal class UserInfoHttpWriter : IHttpResponseWriter<UserInfoResult>
{
    public async Task WriteHttpResponse(UserInfoResult result, HttpContext context)
    {
        context.Response.SetNoCache();
        await context.Response.WriteJsonAsync(result.Claims);
    }
}
