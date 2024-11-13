// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;

namespace IdentityServer4.Models;

/// <summary>
/// Represents a persisted Pushed Authorization Request.
/// </summary>
public class PushedAuthorizationRequest
{
    /// <summary>
    /// The hash of the identifier within this pushed request's request_uri
    /// value. Request URIs that IdentityServer produces take the form
    /// urn:ietf:params:oauth:request_uri:{ReferenceValue}. 
    /// </summary>
    public string ReferenceValueHash { get; set; }

    /// <summary>
    /// The UTC time at which this pushed request will expire. The Pushed
    /// request will be used throughout the authentication process, beginning
    /// when it is passed to the authorization endpoint by the client, and then
    /// subsequently after user interaction, such as login and/or consent occur.
    /// If the expiration time is exceeded before a response to the client can
    /// be produced, IdentityServer will raise an error, and the user will be
    /// redirected to the IdentityServer error page. 
    /// </summary>

    public DateTime ExpiresAtUtc { get; set; }

    /// <summary>
    /// The data protected content of the pushed authorization request.  
    /// </summary>
    public string Parameters { get; set; }
}
