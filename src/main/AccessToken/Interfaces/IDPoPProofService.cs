// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;

namespace IdentityServer4.AccessToken;

/// <summary>
/// Service to create DPoP proof tokens
/// </summary>
public interface IDPoPProofService
{
    /// <summary>
    /// Creates DPoP proof token
    /// </summary>
    Task<DPoPProof?> CreateProofTokenAsync(DPoPProofRequest request);

    /// <summary>
    /// Gets the thumbprint from the JSON web key
    /// </summary>
    string? GetProofKeyThumbprint(DPoPProofRequest request);
}

/// <summary>
/// Models the request information to create a DPoP proof token
/// </summary>
public class DPoPProofRequest
{
    /// <summary>
    /// The HTTP URL of the request
    /// </summary>
    public string Url { get; set; } = default!;

    /// <summary>
    /// The HTTP method of the request
    /// </summary>
    public string Method { get; set; } = default!;

    /// <summary>
    /// The string representation of the JSON web key to use for DPoP.
    /// </summary>
    public string DPoPJsonWebKey { get; set; } = default!;

    /// <summary>
    /// The nonce value for the DPoP proof token.
    /// </summary>
    public string? DPoPNonce { get; set; }

    /// <summary>
    /// The access token
    /// </summary>
    public string? AccessToken { get; set; }
}

/// <summary>
/// Models a DPoP proof token
/// </summary>
public class DPoPProof
{
    /// <summary>
    /// The proof token
    /// </summary>
    public string ProofToken { get; set; } = default!;
}
