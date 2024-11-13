// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;

namespace IdentityServer4.AccessToken;

/// <summary>
/// Represents a client access token
/// </summary>
public class ClientCredentialsToken
{
    /// <summary>
    /// The access token
    /// </summary>
    public string? AccessToken { get; set; }

    /// <summary>
    /// The access token type
    /// </summary>
    public string? AccessTokenType { get; set; }

    /// <summary>
    /// The string representation of the JSON web key to use for DPoP.
    /// </summary>
    public string? DPoPJsonWebKey { get; set; }

    /// <summary>
    /// The access token expiration
    /// </summary>
    public DateTimeOffset Expiration { get; set; }

    /// <summary>
    /// The scope of the access tokens
    /// </summary>
    public string? Scope { get; set; }

    /// <summary>
    /// Error (if any) during token request
    /// </summary>
    public string? Error { get; set; }

    /// <summary>
    /// Checks for an error
    /// </summary>
    public bool IsError => !string.IsNullOrWhiteSpace(Error);
}
