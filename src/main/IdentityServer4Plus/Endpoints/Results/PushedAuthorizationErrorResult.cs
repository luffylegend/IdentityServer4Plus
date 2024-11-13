// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Extensions;
using IdentityServer4.Hosting;
using IdentityServer4.ResponseHandling;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace IdentityServer4.Endpoints.Results;

/// <summary>
/// Represents an error result from the pushed authorization endpoint that can be written to the http response.
/// </summary>
public class PushedAuthorizationErrorResult : EndpointResult<PushedAuthorizationErrorResult>
{

    /// <summary>
    /// The error response model.
    /// </summary>
    public PushedAuthorizationFailure Response { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PushedAuthorizationErrorResult"/> class.
    /// </summary>
    /// <param name="response">The error response model.</param>
    public PushedAuthorizationErrorResult(PushedAuthorizationFailure response)
    {
        Response = response ?? throw new ArgumentNullException(nameof(response));
    }
}

internal class PushedAuthorizationErrorHttpWriter : IHttpResponseWriter<PushedAuthorizationErrorResult>
{
    public async Task WriteHttpResponse(PushedAuthorizationErrorResult result, HttpContext context)
    {
        context.Response.SetNoCache();
        context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
        var dto = new ResultDto
        {
            error = result.Response.Error,
            error_description = result.Response.ErrorDescription,
        };

        await context.Response.WriteJsonAsync(dto);
    }

    internal class ResultDto
    {
#pragma warning disable IDE1006 // Naming Styles
        public string error { get; set; }
        public string error_description { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }
}
