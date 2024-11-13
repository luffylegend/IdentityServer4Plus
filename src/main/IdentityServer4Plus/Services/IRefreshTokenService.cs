// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


#nullable enable

using IdentityServer4.Models;
using IdentityServer4.Validation;
using System.Threading.Tasks;

namespace IdentityServer4.Services;

/// <summary>
/// Implements refresh token creation and validation
/// </summary>
public interface IRefreshTokenService
{
    /// <summary>
    /// Validates a refresh token.
    /// </summary>
    /// <param name="token">The refresh token.</param>
    /// <param name="client">The client.</param>
    /// <returns></returns>
    Task<TokenValidationResult> ValidateRefreshTokenAsync(string token, Client client);

    /// <summary>
    /// Creates the refresh token.
    /// </summary>
    /// <returns>
    /// The refresh token handle
    /// </returns>
    Task<string> CreateRefreshTokenAsync(RefreshTokenCreationRequest request);

    /// <summary>
    /// Updates the refresh token.
    /// </summary>
    /// <returns>
    /// The refresh token handle
    /// </returns>
    Task<string> UpdateRefreshTokenAsync(RefreshTokenUpdateRequest request);
}
