// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Hosting;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace IdentityServer4.Endpoints.Results;

/// <summary>
/// Result for a raw HTTP status code
/// </summary>
/// <seealso cref="IEndpointResult" />
public class StatusCodeResult : EndpointResult<StatusCodeResult>
{
    /// <summary>
    /// Gets the status code.
    /// </summary>
    /// <value>
    /// The status code.
    /// </value>
    public int StatusCode { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StatusCodeResult"/> class.
    /// </summary>
    /// <param name="statusCode">The status code.</param>
    public StatusCodeResult(HttpStatusCode statusCode)
    {
        StatusCode = (int) statusCode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StatusCodeResult"/> class.
    /// </summary>
    /// <param name="statusCode">The status code.</param>
    public StatusCodeResult(int statusCode)
    {
        StatusCode = statusCode;
    }
}

class StatusCodeHttpWriter : IHttpResponseWriter<StatusCodeResult>
{
    public Task WriteHttpResponse(StatusCodeResult result, HttpContext context)
    {
        context.Response.StatusCode = result.StatusCode;

        return Task.CompletedTask;
    }
}
