// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Specialized;
using IdentityServer4.Models;

namespace IdentityServer4.Validation;

/// <summary>
/// Context needed to validate a pushed authorization request.
/// </summary>
public class PushedAuthorizationRequestValidationContext
{
    /// <summary>
    /// Initializes an instance of the <see cref="PushedAuthorizationRequestValidationContext"/> class.
    /// </summary>
    /// <param name="requestParameters">The raw parameters that were passed to the PAR endpoint.</param>
    /// <param name="client">The client that made the request.</param>
    public PushedAuthorizationRequestValidationContext(NameValueCollection requestParameters, Client client)
    {
        RequestParameters = requestParameters;
        Client = client;
    }
    /// <summary>
    /// The request form parameters
    /// </summary>
    public NameValueCollection RequestParameters { get; set; }

    /// <summary>
    /// The validation result of client authentication
    /// </summary>
    public Client Client { get; set; }
}
