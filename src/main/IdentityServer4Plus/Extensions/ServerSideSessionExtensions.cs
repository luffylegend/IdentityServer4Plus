// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Diagnostics;

namespace IdentityServer4.Extensions;

/// <summary>
/// Extensions for ServerSideSession
/// </summary>
internal static class ServerSideSessionExtensions
{
    /// <summary>
    /// Clones the instance
    /// </summary>
    [DebuggerStepThrough]
    internal static ServerSideSession Clone(this ServerSideSession other)
    {
        var item = new ServerSideSession()
        {
            Key = other.Key,
            Scheme = other.Scheme,
            SubjectId = other.SubjectId,
            SessionId = other.SessionId,
            DisplayName = other.DisplayName,
            Created = other.Created,
            Renewed = other.Renewed,
            Expires = other.Expires,
            Ticket = other.Ticket,
        };
        return item;
    }
}
