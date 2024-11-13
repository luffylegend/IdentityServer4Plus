// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer4.AccessToken;

/// <summary>
/// Implements token management logic
/// </summary>
public class ClientCredentialsTokenManagementService : IClientCredentialsTokenManagementService
{
    private readonly ITokenRequestSynchronization _sync;
    private readonly IClientCredentialsTokenEndpointService _clientCredentialsTokenEndpointService;
    private readonly IClientCredentialsTokenCache _tokenCache;
    private readonly ILogger<ClientCredentialsTokenManagementService> _logger;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="sync"></param>
    /// <param name="clientCredentialsTokenEndpointService"></param>
    /// <param name="tokenCache"></param>
    /// <param name="logger"></param>
    public ClientCredentialsTokenManagementService(
        ITokenRequestSynchronization sync,
        IClientCredentialsTokenEndpointService clientCredentialsTokenEndpointService,
        IClientCredentialsTokenCache tokenCache,
        ILogger<ClientCredentialsTokenManagementService> logger)
    {
        _sync = sync;
        _clientCredentialsTokenEndpointService = clientCredentialsTokenEndpointService;
        _tokenCache = tokenCache;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<ClientCredentialsToken> GetAccessTokenAsync(
        string clientName,
        TokenRequestParameters? parameters = null,
        CancellationToken cancellationToken = default)
    {
        parameters ??= new TokenRequestParameters();

        if (parameters.ForceRenewal == false)
        {
            try
            {
                var item = await _tokenCache.GetAsync(clientName, parameters, cancellationToken).ConfigureAwait(false);
                if (item != null)
                {
                    return item;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                    "Error trying to obtain token from cache for client {clientName}. Error = {error}. Will obtain new token.", 
                    clientName, e.Message);
            }
        }

        return await _sync.SynchronizeAsync(clientName, async () =>
        {
            var token = await _clientCredentialsTokenEndpointService.RequestToken(clientName, parameters, cancellationToken).ConfigureAwait(false);
            if (token.IsError)
            {
                _logger.LogError(
                    "Error requesting access token for client {clientName}. Error = {error}.",
                    clientName, token.Error);

                return token;
            }

            try
            {
                await _tokenCache.SetAsync(clientName, token, parameters, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                    "Error trying to set token in cache for client {clientName}. Error = {error}", 
                    clientName, e.Message);
            }

            return token;
        }).ConfigureAwait(false);
    }


    /// <inheritdoc/>
    public Task DeleteAccessTokenAsync(
        string clientName,
        TokenRequestParameters? parameters = null,
        CancellationToken cancellationToken = default)
    {
        parameters ??= new TokenRequestParameters();
        return _tokenCache.DeleteAsync(clientName, parameters, cancellationToken);
    }
}
