// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.EntityFramework.Services;

/// <summary>
/// Implementation of ICorsPolicyService that consults the client configuration in the database for allowed CORS origins.
/// </summary>
/// <seealso cref="ICorsPolicyService" />
public class CorsPolicyService : ICorsPolicyService
{
    /// <summary>
    /// The DbContext.
    /// </summary>
    protected readonly IConfigurationDbContext DbContext;

    /// <summary>
    /// The CancellationToken provider.
    /// </summary>
    protected readonly ICancellationTokenProvider CancellationTokenProvider;

    /// <summary>
    /// The logger.
    /// </summary>
    protected readonly ILogger<CorsPolicyService> Logger;


    /// <summary>
    /// Initializes a new instance of the <see cref="CorsPolicyService"/> class.
    /// </summary>
    /// <param name="dbContext">The DbContext</param>
    /// <param name="logger">The logger.</param>
    /// <param name="cancellationTokenProvider"></param>
    /// <exception cref="ArgumentNullException">context</exception>
    public CorsPolicyService(IConfigurationDbContext dbContext, ILogger<CorsPolicyService> logger, ICancellationTokenProvider cancellationTokenProvider)
    {
        DbContext = dbContext;
        Logger = logger;
        CancellationTokenProvider = cancellationTokenProvider;
    }

    /// <summary>
    /// Determines whether origin is allowed.
    /// </summary>
    /// <param name="origin">The origin.</param>
    /// <returns></returns>
    public async Task<bool> IsOriginAllowedAsync(string origin)
    {
        origin = origin.ToLowerInvariant();

        var query = from o in DbContext.ClientCorsOrigins
                    where o.Origin == origin
                    select o;

        var isAllowed = await query.AnyAsync(CancellationTokenProvider.CancellationToken);

        Logger.LogDebug("Origin {origin} is allowed: {originAllowed}", origin, isAllowed);

        return isAllowed;
    }
}
