// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


#nullable enable

using System;

namespace IdentityServer4.Stores;

/// <summary>
/// Filter to query user sessions
/// </summary>
public class SessionFilter
{
    /// <summary>
    /// The subject ID
    /// </summary>
    public string? SubjectId { get; init; }

    /// <summary>
    /// The session ID
    /// </summary>
    public string? SessionId { get; init; }

    /// <summary>
    /// Validates
    /// </summary>
    public void Validate()
    {
        if (String.IsNullOrWhiteSpace(SubjectId) && String.IsNullOrWhiteSpace(SessionId))
        {
            throw new ArgumentNullException("SubjectId or SessionId is required.");
        }
    }
}
