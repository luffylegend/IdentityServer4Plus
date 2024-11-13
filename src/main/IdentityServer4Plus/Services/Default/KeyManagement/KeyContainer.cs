// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer4.Services.KeyManagement;

/// <summary>
/// Container class for key.
/// </summary>
public abstract class KeyContainer
{
    /// <summary>
    /// Constructor for KeyContainer.
    /// </summary>
    public KeyContainer()
    {
    }

    /// <summary>
    /// Constructor for RsaKeyContainer.
    /// </summary>
    public KeyContainer(string id, string algorithm, DateTime created)
    {
        Id = id;
        Algorithm = algorithm;
        Created = created;
    }

    /// <summary>
    /// Key identifier.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The algorithm this key supports.
    /// </summary>
    public string Algorithm { get; set; }

    /// <summary>
    /// Indicates if key is contained in X509 certificate.
    /// </summary>
    public bool HasX509Certificate { get; set; }

    /// <summary>
    /// Date key was created.
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// Creates AsymmetricSecurityKey.
    /// </summary>
    /// <returns></returns>
    public abstract AsymmetricSecurityKey ToSecurityKey();
}
