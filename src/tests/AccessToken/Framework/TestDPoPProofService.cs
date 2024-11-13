﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace IdentityServer4.AccessToken.Tests;

public class TestDPoPProofService : IDPoPProofService
{
    public string? ProofToken { get; set; }
    public string? Nonce { get; set; }
    public bool AppendNonce { get; set; }

    public Task<DPoPProof?> CreateProofTokenAsync(DPoPProofRequest request)
    {
        if (ProofToken == null) return Task.FromResult<DPoPProof?>(null);
        Nonce = request.DPoPNonce;
        return Task.FromResult<DPoPProof?>(new DPoPProof { ProofToken = ProofToken + Nonce });
    }

    public string? GetProofKeyThumbprint(DPoPProofRequest request)
    {
        return null;
    }
}
