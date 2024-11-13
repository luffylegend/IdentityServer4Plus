// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


#nullable enable

namespace IdentityServer4.Models;

/// <summary>
/// Models the data to update a refresh token.
/// </summary>
public class RefreshTokenUpdateRequest
{
    /// <summary>
    /// The handle of the refresh token.
    /// </summary>
    public string Handle { get; set; } = default!;

    /// <summary>
    /// The client.
    /// </summary>
    public Client Client { get; set; } = default!;

    /// <summary>
    /// The refresh token to update.
    /// </summary>
    public RefreshToken RefreshToken { get; set; } = default!;

    /// <summary>
    /// Flag to indicate that the refreth token was modified, and requires an update to the database.
    /// </summary>
    public bool MustUpdate { get; set; }
}
