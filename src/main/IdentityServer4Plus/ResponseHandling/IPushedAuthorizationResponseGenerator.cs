// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


#nullable enable

using System.Threading.Tasks;
using IdentityServer4.Validation;

namespace IdentityServer4.ResponseHandling;


/// <summary>
/// Service that generates response models for the pushed authorization endpoint. This service encapsulates the behavior that
/// is needed to create a response model from a validated request. 
/// </summary>
public interface IPushedAuthorizationResponseGenerator
{
    /// <summary>
    /// Asynchronously creates a response model from a validated pushed authorization request.
    /// </summary>
    /// <param name="request">The validated pushed authorization request.</param>
    /// <returns>A task that contains response model indicating either success or failure.</returns>
    Task<PushedAuthorizationResponse> CreateResponseAsync(ValidatedPushedAuthorizationRequest request);
}
