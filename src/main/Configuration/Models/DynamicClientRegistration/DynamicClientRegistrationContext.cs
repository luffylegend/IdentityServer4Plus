// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Security.Claims;
using IdentityServer4.Models;

namespace IdentityServer4.Configuration.Models.DynamicClientRegistration;

/// <summary>
/// Represents the context for a dynamic client registration request, including
/// the original DCR request, the client model that is built up through
/// validation and processing, the caller who made the DCR request, and other
/// contextual information.
/// </summary>
public class DynamicClientRegistrationContext
{
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="DynamicClientRegistrationContext"/> class.
    /// </summary>
    /// <param name="request">The original dynamic client registration request.</param>
    /// <param name="caller">The <see cref="ClaimsPrincipal"/> that made the DCR request.</param>
    public DynamicClientRegistrationContext(DynamicClientRegistrationRequest request, ClaimsPrincipal caller)
    {
        Request = request;
        Caller = caller;
    }

    /// <summary>
    /// The client model that is built up through validation and processing.
    /// </summary>
    public Client Client { get; set; } = new();

    /// <summary>
    /// The original dynamic client registration request.
    /// </summary>
    public DynamicClientRegistrationRequest Request { get; set; }

    /// <summary>
    /// The <see cref="ClaimsPrincipal"/> that made the DCR request.
    /// </summary>
    public ClaimsPrincipal Caller { get; set; }

    /// <summary>
    /// A collection where additional contextual information may be stored. This
    /// is intended as a place to pass additional custom state between
    /// validation steps.
    /// </summary>
    public Dictionary<string, object> Items { get; set; } = new();
}
