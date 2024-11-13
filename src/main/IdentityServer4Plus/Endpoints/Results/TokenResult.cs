// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Hosting;
using IdentityServer4.ResponseHandling;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IdentityServer4.Endpoints.Results;

/// <summary>
/// Models a token result
/// </summary>
public class TokenResult : EndpointResult<TokenResult>
{
    /// <summary>
    /// The response
    /// </summary>
    public TokenResponse Response { get; }

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="response"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public TokenResult(TokenResponse response)
    {
        Response = response ?? throw new ArgumentNullException(nameof(response));
    }
}

internal class TokenHttpWriter : IHttpResponseWriter<TokenResult>
{
    public async Task WriteHttpResponse(TokenResult result, HttpContext context)
    {
        context.Response.SetNoCache();

        if (result.Response.DPoPNonce.IsPresent())
        {
            context.Response.Headers[OidcConstants.HttpHeaders.DPoPNonce] = result.Response.DPoPNonce;
        }

        var dto = new ResultDto
        {
            id_token = result.Response.IdentityToken,
            access_token = result.Response.AccessToken,
            refresh_token = result.Response.RefreshToken,
            expires_in = result.Response.AccessTokenLifetime,
            token_type = result.Response.AccessTokenType,
            scope = result.Response.Scope,

            Custom = result.Response.Custom
        };

        await context.Response.WriteJsonAsync(dto);
    }

    internal class ResultDto
    {
#pragma warning disable IDE1006 // Naming Styles
        public string id_token { get; set; }
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
        public string refresh_token { get; set; }
        public string scope { get; set; }

        [JsonExtensionData]
        public Dictionary<string, object> Custom { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }
}
