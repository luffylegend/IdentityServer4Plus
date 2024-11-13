// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace IdentityServer4.Hosting;

/// <summary>
/// Contract for a service that writes appropriate http responses for <see
/// cref="IEndpointResult"/> objects.
/// </summary>
public interface IHttpResponseWriter<in T>
    where T : IEndpointResult
{
    /// <summary>
    /// Writes the endpoint result to the HTTP response.
    /// </summary>
    Task WriteHttpResponse(T result, HttpContext context);
}
