// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


#nullable enable

using System;
using System.Collections.Specialized;

namespace IdentityServer4.Services;

/// <summary>
/// A pushed authorization request that is not serialized.
/// </summary>
public class DeserializedPushedAuthorizationRequest
{
    /// <summary>
    /// The reference value of the pushed authorization request. This is the
    /// identifier within the request_uri.
    /// </summary>
    public required string ReferenceValue { get; set; }

    /// <summary>
    /// The pushed parameters. 
    /// </summary>
    public required NameValueCollection PushedParameters { get; set; }

    /// <summary>
    /// The expiration time.
    /// </summary>
    public required DateTime ExpiresAtUtc { get; set; }
}
