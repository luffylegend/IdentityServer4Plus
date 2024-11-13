// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;

namespace IdentityServer4.Validation;

/// <summary>
/// Default implementation of the CIBA validator extensibility point. This
/// validator deliberately does nothing.
/// </summary>
public class DefaultCustomBackchannelAuthenticationValidator : ICustomBackchannelAuthenticationValidator
{
    /// <inheritdoc/>
    public Task ValidateAsync(CustomBackchannelAuthenticationRequestValidationContext customValidationContext)
    {
        return Task.CompletedTask;
    }
}
