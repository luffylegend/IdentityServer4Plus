// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Threading.Tasks;
using IdentityServer4.Configuration;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.Extensions.Logging;

namespace IdentityServer4.ResponseHandling;

/// <inheritdoc />
public class PushedAuthorizationResponseGenerator : IPushedAuthorizationResponseGenerator
{
    private readonly IHandleGenerationService _handleGeneration;
    private readonly IdentityServerOptions _options;
    private readonly IPushedAuthorizationService _pushedAuthorizationService;
    private readonly ILogger<PushedAuthorizationResponseGenerator> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PushedAuthorizationResponseGenerator"/> class.
    /// </summary>
    /// <param name="handleGeneration">The handle generation service, used for creation of request uri reference values.
    /// </param>
    /// <param name="options">The IdentityServer options</param>
    /// <param name="pushedAuthorizationService">The pushed authorization service</param>
    /// <param name="logger">The logger</param>
    public PushedAuthorizationResponseGenerator(IHandleGenerationService handleGeneration,
        IdentityServerOptions options,
        IPushedAuthorizationService pushedAuthorizationService,
        ILogger<PushedAuthorizationResponseGenerator> logger)
    {
        _handleGeneration = handleGeneration;
        _options = options;
        _pushedAuthorizationService = pushedAuthorizationService;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<PushedAuthorizationResponse> CreateResponseAsync(ValidatedPushedAuthorizationRequest request)
    {
        // Create a reference value
        var referenceValue = await _handleGeneration.GenerateAsync();

        var requestUri = $"{IdentityServerConstants.PushedAuthorizationRequestUri}:{referenceValue}";

        // Calculate the expiration
        var expiration = request.Client.PushedAuthorizationLifetime ?? _options.PushedAuthorization.Lifetime;
        var expiresAt = DateTime.UtcNow.AddSeconds(expiration);

        await _pushedAuthorizationService.StoreAsync(new DeserializedPushedAuthorizationRequest
        {
            ReferenceValue = referenceValue,
            ExpiresAtUtc = expiresAt,
            PushedParameters = request.Raw
        });

        // Return reference and expiration
        return new PushedAuthorizationSuccess
        {
            RequestUri = requestUri,
            ExpiresIn = expiration
        };
    }
}
