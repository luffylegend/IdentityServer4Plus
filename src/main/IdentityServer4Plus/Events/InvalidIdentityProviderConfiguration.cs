// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;

namespace IdentityServer4.Events;

/// <summary>
/// Event for unhandled exceptions
/// </summary>
/// <seealso cref="Event" />
public class InvalidIdentityProviderConfiguration : Event
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidIdentityProviderConfiguration" /> class.
    /// </summary>
    public InvalidIdentityProviderConfiguration(IdentityProvider idp, string errorMessage)
        : base(EventCategories.Error,
            "Invalid IdentityProvider Configuration",
            EventTypes.Error,
            EventIds.InvalidIdentityProviderConfiguration,
            errorMessage)
    {
        Scheme = idp.Scheme;
        DisplayName = idp.DisplayName ?? "unknown name";
        Type = idp.Type ?? "unknown type";
    }

    /// <summary>
    /// Gets or sets the scheme.
    /// </summary>
    public string Scheme { get; set; }

    /// <summary>
    /// Gets or sets the display name of the identity provider.
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the type of the identity provider.
    /// </summary>
    public string Type { get; set; }
}
