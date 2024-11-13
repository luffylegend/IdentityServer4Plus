// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


#nullable enable

using System.Threading.Tasks;

namespace IdentityServer4.Validation;

/// <summary>
/// Validator for handling identity provider configuration
/// </summary>
public interface IIdentityProviderConfigurationValidator
{
    /// <summary>
    /// Determines whether the configuration of an identity provider is valid.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    Task ValidateAsync(IdentityProviderConfigurationValidationContext context);
}
