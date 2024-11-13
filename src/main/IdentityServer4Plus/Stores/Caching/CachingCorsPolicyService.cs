// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Configuration;
using IdentityServer4.Services;
using System.Threading.Tasks;

namespace IdentityServer4.Stores;

/// <summary>
/// Caching decorator for ICorsPolicyService
/// </summary>
/// <seealso cref="ICorsPolicyService" />
public class CachingCorsPolicyService<T> : ICorsPolicyService
where T : ICorsPolicyService
{
    /// <summary>
    /// Class to model entries in CORS origin cache.
    /// </summary>
    public class CorsCacheEntry
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        public CorsCacheEntry(bool allowed)
        {
            Allowed = allowed;
        }

        /// <summary>
        /// Is origin allowed.
        /// </summary>
        public bool Allowed { get; }
    }

    private readonly IdentityServerOptions _options;
    private readonly ICache<CorsCacheEntry> _corsCache;
    private readonly ICorsPolicyService _inner;

    /// <summary>
    /// Initializes a new instance of the <see cref="CachingResourceStore{T}"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <param name="inner">The inner.</param>
    /// <param name="corsCache">The CORS origin cache.</param>
    public CachingCorsPolicyService(IdentityServerOptions options,
        T inner,
        ICache<CorsCacheEntry> corsCache)
    {
        _options = options;
        _inner = inner;
        _corsCache = corsCache;
    }

    /// <summary>
    /// Determines whether origin is allowed.
    /// </summary>
    /// <param name="origin">The origin.</param>
    /// <returns></returns>
    public virtual async Task<bool> IsOriginAllowedAsync(string origin)
    {
        using var activity = Tracing.StoreActivitySource.StartActivity("CachingCorsPolicyService.IsOriginAllowed");
        activity?.SetTag(Tracing.Properties.Origin, origin);

        var entry = await _corsCache.GetOrAddAsync(origin,
            _options.Caching.CorsExpiration,
            async () => new CorsCacheEntry(await _inner.IsOriginAllowedAsync(origin)));

        return entry.Allowed;
    }
}
