// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;

namespace IdentityServer4.Validation;

/// <summary>
/// Models the thumbprint of a proof key
/// </summary>
public class ProofKeyThumbprint
{
    /// <summary>
    /// The type
    /// </summary>
    public ProofType Type { get; set; }
    /// <summary>
    /// The thumbprint value used in a cnf thumbprint claim value
    /// </summary>
    public string Thumbprint { get; set; }
}
