// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


#nullable enable

using IdentityServer4.Models;

namespace IdentityServer4.Validation;


/// <summary>
/// Context for identity provider configuration validation.
/// </summary>
public class IdentityProviderConfigurationValidationContext : IdentityProviderConfigurationValidationContext<IdentityProvider>
{
    /// <summary>
    /// Initializes a new instance of the IdentityProviderConfigurationValidationContext class.
    /// </summary>
    public IdentityProviderConfigurationValidationContext(IdentityProvider idp) : base(idp)
    {
    }
}

/// <summary>
/// Context for identity provider configuration validation.
/// </summary>
public class IdentityProviderConfigurationValidationContext<T>
    where T : IdentityProvider
{
    /// <summary>
    /// Gets or sets the identity provider.
    /// </summary>
    public T IdentityProvider { get; }

    /// <summary>
    /// Returns true if the configuration is valid.
    /// </summary>
    public bool IsValid { get; set; } = true;

    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Initializes a new instance of the IdentityProviderConfigurationValidationContext class.
    /// </summary>
    public IdentityProviderConfigurationValidationContext(T idp)
    {
        IdentityProvider = idp;
    }

    /// <summary>
    /// Sets a validation error.
    /// </summary>
    /// <param name="message">The message.</param>
    public void SetError(string message)
    {
        IsValid = false;
        ErrorMessage = message;
    }
}
