// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


#nullable enable

using IdentityServer4.Models;
using System.Threading.Tasks;

namespace IdentityServer4.Services;

/// <summary>
/// Interface for sending a user a login request from a backchannel authentication request.
/// </summary>
public interface IBackchannelAuthenticationUserNotificationService
{
    /// <summary>
    /// Sends a notification for the user to login.
    /// </summary>
    Task SendLoginRequestAsync(BackchannelUserLoginRequest request);
}
