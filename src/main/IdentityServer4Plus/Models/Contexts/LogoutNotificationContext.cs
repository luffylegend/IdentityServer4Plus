// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


#nullable enable

using System.Collections.Generic;

namespace IdentityServer4.Models;

/// <summary>
/// Provides the context necessary to construct a logout notification.
/// </summary>
public class LogoutNotificationContext
{
    /// <summary>
    ///  The SubjectId of the user.
    /// </summary>
    public string SubjectId { get; set; } = default!;

    /// <summary>
    /// The session Id of the user's authentication session.
    /// </summary>
    public string SessionId { get; set; } = default!;

    /// <summary>
    /// The issuer for the back-channel logout
    /// </summary>
    public string? Issuer { get; set; }

    /// <summary>
    /// The list of client Ids that the user has authenticated to.
    /// </summary>
    public IEnumerable<string> ClientIds { get; set; } = default!;

    /// <summary>
    /// Indicates why the user's session ended, if known.
    /// </summary>
    public LogoutNotificationReason? LogoutReason { get; set; }
}
