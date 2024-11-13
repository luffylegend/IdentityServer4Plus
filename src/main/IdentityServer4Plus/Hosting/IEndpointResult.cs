// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace IdentityServer4.Hosting;

/// <summary>
/// An <see cref="IEndpointResult"/> is the object model that describes the
/// results that will returned by one of the protocol endpoints provided by
/// IdentityServer, and can be executed to produce an HTTP response.
/// </summary>
public interface IEndpointResult
{
    /// <summary>
    /// Executes the result to write an http response.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    Task ExecuteAsync(HttpContext context);
}
