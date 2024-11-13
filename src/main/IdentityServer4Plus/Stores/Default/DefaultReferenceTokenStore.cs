// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores.Serialization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace IdentityServer4.Stores;

/// <summary>
/// Default reference token store.
/// </summary>
public class DefaultReferenceTokenStore : DefaultGrantStore<Token>, IReferenceTokenStore
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultReferenceTokenStore"/> class.
    /// </summary>
    /// <param name="store">The store.</param>
    /// <param name="serializer">The serializer.</param>
    /// <param name="handleGenerationService">The handle generation service.</param>
    /// <param name="logger">The logger.</param>
    public DefaultReferenceTokenStore(
        IPersistedGrantStore store,
        IPersistentGrantSerializer serializer,
        IHandleGenerationService handleGenerationService,
        ILogger<DefaultReferenceTokenStore> logger)
        : base(IdentityServerConstants.PersistedGrantTypes.ReferenceToken, store, serializer, handleGenerationService, logger)
    {
    }

    /// <inheritdoc/>
    public Task<string> StoreReferenceTokenAsync(Token token)
    {
        using var activity = Tracing.StoreActivitySource.StartActivity("DefaultReferenceTokenStore.StoreReferenceToken");

        return CreateItemAsync(token, token.ClientId, token.SubjectId, token.SessionId, token.Description, token.CreationTime, token.Lifetime);
    }

    /// <inheritdoc/>
    public Task<Token> GetReferenceTokenAsync(string handle)
    {
        using var activity = Tracing.StoreActivitySource.StartActivity("DefaultReferenceTokenStore.GetReferenceToken");

        return GetItemAsync(handle);
    }

    /// <inheritdoc/>
    public Task RemoveReferenceTokenAsync(string handle)
    {
        using var activity = Tracing.StoreActivitySource.StartActivity("DefaultReferenceTokenStore.RemoveReferenceToken");

        return RemoveItemAsync(handle);
    }

    /// <inheritdoc/>
    public Task RemoveReferenceTokensAsync(string subjectId, string clientId, string sessionId = null)
    {
        using var activity = Tracing.StoreActivitySource.StartActivity("DefaultReferenceTokenStore.RemoveReferenceTokens");

        return RemoveAllAsync(subjectId, clientId, sessionId);
    }
}
