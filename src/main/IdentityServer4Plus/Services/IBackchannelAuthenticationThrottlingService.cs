// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


#nullable enable

using System.Threading.Tasks;
using IdentityServer4.Models;

namespace IdentityServer4.Services;

/// <summary>
/// The backchannel authentication throttling service.
/// </summary>
public interface IBackchannelAuthenticationThrottlingService
{
    /// <summary>
    /// Decides if the requesting client and request needs to slow down.
    /// </summary>
    Task<bool> ShouldSlowDown(string requestId, BackChannelAuthenticationRequest details);
}
