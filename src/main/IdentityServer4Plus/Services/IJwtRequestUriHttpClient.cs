// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


#nullable enable

using IdentityServer4.Models;
using System.Threading.Tasks;

namespace IdentityServer4.Services;

/// <summary>
/// Models making HTTP requests for JWTs from the authorize endpoint.
/// </summary>
public interface IJwtRequestUriHttpClient
{
    /// <summary>
    /// Gets a JWT from the url.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="client"></param>
    /// <returns></returns>
    Task<string?> GetJwtAsync(string url, Client client);
}
