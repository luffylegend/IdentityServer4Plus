// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.EntityFramework.Stores;

/// <summary>
/// Implementation of IPersistedGrantStore thats uses EF.
/// </summary>
/// <seealso cref="IPersistedGrantStore" />
public class PersistedGrantStore : IdentityServer4.Stores.IPersistedGrantStore
{
    /// <summary>
    /// The DbContext.
    /// </summary>
    protected readonly IPersistedGrantDbContext Context;

    /// <summary>
    /// The CancellationToken service.
    /// </summary>
    protected readonly ICancellationTokenProvider CancellationTokenProvider;

    /// <summary>
    /// The logger.
    /// </summary>
    protected readonly ILogger Logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PersistedGrantStore"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="cancellationTokenProvider"></param>
    public PersistedGrantStore(IPersistedGrantDbContext context, ILogger<PersistedGrantStore> logger, ICancellationTokenProvider cancellationTokenProvider)
    {
        Context = context;
        Logger = logger;
        CancellationTokenProvider = cancellationTokenProvider;
    }

    /// <inheritdoc/>
    public virtual async Task StoreAsync(IdentityServer4.Models.PersistedGrant token)
    {
        using var activity = Tracing.StoreActivitySource.StartActivity("PersistedGrantStore.Store");

        var existing = (await Context.PersistedGrants.Where(x => x.Key == token.Key)
                .ToArrayAsync(CancellationTokenProvider.CancellationToken))
            .SingleOrDefault(x => x.Key == token.Key);
        if (existing == null)
        {
            Logger.LogDebug("{persistedGrantKey} not found in database", token.Key);

            var persistedGrant = token.ToEntity();
            Context.PersistedGrants.Add(persistedGrant);
        }
        else
        {
            Logger.LogDebug("{persistedGrantKey} found in database", token.Key);

            token.UpdateEntity(existing);
        }

        try
        {
            await Context.SaveChangesAsync(CancellationTokenProvider.CancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Logger.LogWarning("exception updating {persistedGrantKey} persisted grant in database: {error}", token.Key, ex.Message);
        }
    }

    /// <inheritdoc/>
    public virtual async Task<IdentityServer4.Models.PersistedGrant> GetAsync(string key)
    {
        using var activity = Tracing.StoreActivitySource.StartActivity("PersistedGrantStore.Get");

        var persistedGrant = (await Context.PersistedGrants.AsNoTracking().Where(x => x.Key == key)
                .ToArrayAsync(CancellationTokenProvider.CancellationToken))
            .SingleOrDefault(x => x.Key == key);
        var model = persistedGrant?.ToModel();

        Logger.LogDebug("{persistedGrantKey} found in database: {persistedGrantKeyFound}", key, model != null);

        return model;
    }

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<IdentityServer4.Models.PersistedGrant>> GetAllAsync(PersistedGrantFilter filter)
    {
        using var activity = Tracing.StoreActivitySource.StartActivity("PersistedGrantStore.GetAll");

        filter.Validate();

        var persistedGrants = await Filter(Context.PersistedGrants.AsQueryable(), filter)
            .ToArrayAsync(CancellationTokenProvider.CancellationToken);
        persistedGrants = Filter(persistedGrants.AsQueryable(), filter).ToArray();

        var model = persistedGrants.Select(x => x.ToModel());

        Logger.LogDebug("{persistedGrantCount} persisted grants found for {@filter}", persistedGrants.Length, filter);

        return model;
    }

    /// <inheritdoc/>
    public virtual async Task RemoveAsync(string key)
    {
        using var activity = Tracing.StoreActivitySource.StartActivity("PersistedGrantStore.Remove");

        var persistedGrant = (await Context.PersistedGrants.Where(x => x.Key == key)
                .ToArrayAsync(CancellationTokenProvider.CancellationToken))
            .SingleOrDefault(x => x.Key == key);
        if (persistedGrant != null)
        {
            Logger.LogDebug("removing {persistedGrantKey} persisted grant from database", key);

            Context.PersistedGrants.Remove(persistedGrant);

            try
            {
                await Context.SaveChangesAsync(CancellationTokenProvider.CancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Logger.LogInformation("exception removing {persistedGrantKey} persisted grant from database: {error}", key, ex.Message);
            }
        }
        else
        {
            Logger.LogDebug("no {persistedGrantKey} persisted grant found in database", key);
        }
    }

    /// <inheritdoc/>
    public virtual async Task RemoveAllAsync(PersistedGrantFilter filter)
    {
        using var activity = Tracing.StoreActivitySource.StartActivity("PersistedGrantStore.RemoveAll");

        filter.Validate();

        var persistedGrants = await Filter(Context.PersistedGrants.AsQueryable(), filter)
            .ToArrayAsync(CancellationTokenProvider.CancellationToken);
        persistedGrants = Filter(persistedGrants.AsQueryable(), filter).ToArray();

        Logger.LogDebug("removing {persistedGrantCount} persisted grants from database for {@filter}", persistedGrants.Length, filter);

        Context.PersistedGrants.RemoveRange(persistedGrants);

        try
        {
            await Context.SaveChangesAsync(CancellationTokenProvider.CancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Logger.LogInformation("removing {persistedGrantCount} persisted grants from database for subject {@filter}: {error}", persistedGrants.Length, filter, ex.Message);
        }
    }


    private IQueryable<PersistedGrant> Filter(IQueryable<PersistedGrant> query, PersistedGrantFilter filter)
    {
        if (filter.ClientIds != null)
        {
            var ids = filter.ClientIds.ToList();
            if (!String.IsNullOrWhiteSpace(filter.ClientId))
            {
                ids.Add(filter.ClientId);
            }
            query = query.Where(x => ids.Contains(x.ClientId));
        }
        else if (!String.IsNullOrWhiteSpace(filter.ClientId))
        {
            query = query.Where(x => x.ClientId == filter.ClientId);
        }

        if (!String.IsNullOrWhiteSpace(filter.SessionId))
        {
            query = query.Where(x => x.SessionId == filter.SessionId);
        }
        if (!String.IsNullOrWhiteSpace(filter.SubjectId))
        {
            query = query.Where(x => x.SubjectId == filter.SubjectId);
        }

        if (filter.Types != null)
        {
            var types = filter.Types.ToList();
            if (!String.IsNullOrWhiteSpace(filter.Type))
            {
                types.Add(filter.Type);
            }
            query = query.Where(x => types.Contains(x.Type));
        }
        else if (!String.IsNullOrWhiteSpace(filter.Type))
        {
            query = query.Where(x => x.Type == filter.Type);
        }

        return query;
    }
}
