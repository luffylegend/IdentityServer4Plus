// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Concurrent;
using System.Security.Claims;
using IdentityServer4.AccessToken.OpenIdConnect;

namespace BlazorServer.Plumbing;

/// <summary>
/// Simplified implementation of a server-side token store.
/// Probably want something more robust IRL
/// </summary>
public class ServerSideTokenStore : IUserTokenStore
{
    private readonly ConcurrentDictionary<string, UserToken> _tokens = new();

    public Task<UserToken> GetTokenAsync(ClaimsPrincipal user, UserTokenRequestParameters? parameters = null)
    {
        var sub = user.FindFirst("sub")?.Value ?? throw new InvalidOperationException("no sub claim");

        if (_tokens.TryGetValue(sub, out var value))
        {
            return Task.FromResult(value);
        }

        return Task.FromResult(new UserToken { Error = "not found" });
    }

    public Task StoreTokenAsync(ClaimsPrincipal user, UserToken token, UserTokenRequestParameters? parameters = null)
    {
        var sub = user.FindFirst("sub")?.Value ?? throw new InvalidOperationException("no sub claim");
        _tokens[sub] = token;
    
        return Task.CompletedTask;
    }

    public Task ClearTokenAsync(ClaimsPrincipal user, UserTokenRequestParameters? parameters = null)
    {
        var sub = user.FindFirst("sub")?.Value ?? throw new InvalidOperationException("no sub claim");
    
        _tokens.TryRemove(sub, out _);
        return Task.CompletedTask;
    }
}
