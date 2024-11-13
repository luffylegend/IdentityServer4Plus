// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;

namespace IdentityServer4.ResponseHandling;


/// <summary>
/// Models a backchannel authentication response
/// </summary>
public class BackchannelAuthenticationResponse
{
    /// <summary>
    /// Ctor.
    /// </summary>
    public BackchannelAuthenticationResponse()
    {
    }

    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="error"></param>
    /// <param name="errorDescription"></param>
    public BackchannelAuthenticationResponse(string error, string errorDescription = null)
    {
        Error = error;
        ErrorDescription = errorDescription;
    }

    /// <summary>
    /// Indicates if this response represents an error.
    /// </summary>
    public bool IsError => !String.IsNullOrWhiteSpace(Error);

    /// <summary>
    /// Gets or sets the error.
    /// </summary>
    public string Error { get; set; }

    /// <summary>
    /// Gets or sets the error description.
    /// </summary>
    public string ErrorDescription { get; set; }

    /// <summary>
    /// Gets or sets the authentication request id.
    /// </summary>
    public string AuthenticationRequestId { get; set; }

    /// <summary>
    /// Gets or sets the expires in.
    /// </summary>
    public int ExpiresIn { get; set; }

    /// <summary>
    /// Gets or sets the interval.
    /// </summary>
    public int Interval { get; set; }

    /// <summary> 
    /// Gets or sets a dictionary of custom properties that will be included in
    /// the response to the client. This dictionary is intended to be used to
    /// implement extensions to CIBA that defines additional response
    /// parameters.
    /// </summary>
    public Dictionary<string, object> Custom { get; set; } = new();
}
