// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


#nullable enable

using System;

namespace IdentityServer4.Models;

/// <summary>
/// A user session
/// </summary>
public class ServerSideSession
{
    /// <summary>
    /// The key
    /// </summary>
    public string Key { get; set; } = default!;

    /// <summary>
    /// The cookie handler scheme
    /// </summary>
    public string Scheme { get; set; } = default!;

    /// <summary>
    /// The subject ID
    /// </summary>
    public string SubjectId { get; set; } = default!;

    /// <summary>
    /// The session ID
    /// </summary>
    public string SessionId { get; set; } = default!;

    /// <summary>
    /// The display name for the user
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// The creation time
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// The renewal time
    /// </summary>
    public DateTime Renewed { get; set; }

    /// <summary>
    /// The expiration time
    /// </summary>
    public DateTime? Expires { get; set; }

    /// <summary>
    /// The serialized ticket
    /// </summary>
    public string Ticket { get; set; } = default!;
}
