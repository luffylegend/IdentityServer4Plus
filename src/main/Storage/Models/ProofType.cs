// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace IdentityServer4.Models;

/// <summary>
/// Models the type of proof of possession
/// </summary>
public enum ProofType
{
    /// <summary>
    /// None
    /// </summary>
    None,
    /// <summary>
    /// Client certificate
    /// </summary>
    ClientCertificate,
    /// <summary>
    /// DPoP
    /// </summary>
    DPoP
}
