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
/// Result for introspection
/// </summary>
/// <seealso cref="IEndpointResult" />
public class IntrospectionResult : EndpointResult<IntrospectionResult>
{
    /// <summary>
    /// Gets the result.
    /// </summary>
    /// <value>
    /// The result.
    /// </value>
    public Dictionary<string, object> Entries { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="IntrospectionResult"/> class.
    /// </summary>
    /// <param name="entries">The result.</param>
    /// <exception cref="System.ArgumentNullException">result</exception>
    public IntrospectionResult(Dictionary<string, object> entries)
    {
        Entries = entries ?? throw new ArgumentNullException(nameof(entries));
    }
}


class IntrospectionHttpWriter : IHttpResponseWriter<IntrospectionResult>
{
    public Task WriteHttpResponse(IntrospectionResult result, HttpContext context)
    {
        context.Response.SetNoCache();

        return context.Response.WriteJsonAsync(result.Entries);
    }
}
