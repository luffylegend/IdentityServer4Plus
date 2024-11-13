// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace IdentityServer4.AccessToken;

/// <summary>
/// Default values
/// </summary>
public static class ClientCredentialsTokenManagementDefaults
{
    /// <summary>
    /// Name of the back-channel HTTP client
    /// </summary>
    public const string BackChannelHttpClientName = "IdentityServer4.AccessToken.BackChannelHttpClient";
    
    /// <summary>
    /// Name used to propagate access token parameters to HttpRequestMessage
    /// </summary>
    public const string TokenRequestParametersOptionsName = "IdentityServer4.AccessToken.AccessTokenParameters";
}
