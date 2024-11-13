// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Extensions;
using IdentityServer4.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace IdentityServer4.Endpoints.Results;

/// <summary>
/// Result for revocation error
/// </summary>
/// <seealso cref="IEndpointResult" />
public class TokenRevocationErrorResult : EndpointResult<TokenRevocationErrorResult>
{
    /// <summary>
    /// Gets or sets the error.
    /// </summary>
    /// <value>
    /// The error.
    /// </value>
    public string Error { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenRevocationErrorResult"/> class.
    /// </summary>
    /// <param name="error">The error.</param>
    public TokenRevocationErrorResult(string error)
    {
        Error = error ?? throw new ArgumentNullException(nameof(error));
    }
}

class TokenRevocationErrorHttpWriter : IHttpResponseWriter<TokenRevocationErrorResult>
{
    public Task WriteHttpResponse(TokenRevocationErrorResult result, HttpContext context)
    {
        context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
        return context.Response.WriteJsonAsync(new { error = result.Error });
    }
}
