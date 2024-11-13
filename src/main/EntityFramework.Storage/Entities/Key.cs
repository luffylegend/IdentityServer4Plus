// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

#pragma warning disable 1591

using System;

namespace IdentityServer4.EntityFramework.Entities;

/// <summary>
/// Models storage for keys.
/// </summary>
public class Key
{
    public string Id { get; set; }
    public int Version { get; set; }
    public DateTime Created { get; set; }
    public string Use { get; set; }
    public string Algorithm { get; set; }
    public bool IsX509Certificate { get; set; }
    public bool DataProtected { get; set; }
    public string Data { get; set; }
}
