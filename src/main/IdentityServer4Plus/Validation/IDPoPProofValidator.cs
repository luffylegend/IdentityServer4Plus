// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


#nullable enable

using System.Threading.Tasks;

namespace IdentityServer4.Validation;

/// <summary>
/// Validator for handling DPoP proofs.
/// </summary>
public interface IDPoPProofValidator
{
    /// <summary>
    /// Validates the DPoP proof.
    /// </summary>
    Task<DPoPProofValidatonResult> ValidateAsync(DPoPProofValidatonContext context);
}
