// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Threading.Tasks;
using IdentityServer4.Configuration;
using IdentityServer4.Validation;
using IdentityServer4.Models;
using Microsoft.Extensions.Logging;
using IdentityServer4.Stores;
using IdentityServer4.Services;

namespace IdentityServer4.ResponseHandling;


/// <summary>
/// The backchannel authentication response generator
/// </summary>
/// <seealso cref="IBackchannelAuthenticationResponseGenerator" />
public class BackchannelAuthenticationResponseGenerator : IBackchannelAuthenticationResponseGenerator
{
    /// <summary>
    /// The options
    /// </summary>
    protected readonly IdentityServerOptions Options;

    /// <summary>
    /// The request store.
    /// </summary>
    protected readonly IBackChannelAuthenticationRequestStore BackChannelAuthenticationRequestStore;

    /// <summary>
    /// The user login service.
    /// </summary>
    protected readonly IBackchannelAuthenticationUserNotificationService UserLoginService;

    /// <summary>
    /// The clock
    /// </summary>
    protected readonly IClock Clock;

    /// <summary>
    /// The logger
    /// </summary>
    protected readonly ILogger Logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="BackchannelAuthenticationResponseGenerator"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <param name="backChannelAuthenticationRequestStore"></param>
    /// <param name="userLoginService"></param>
    /// <param name="clock">The clock.</param>
    /// <param name="logger">The logger.</param>
    public BackchannelAuthenticationResponseGenerator(IdentityServerOptions options,
        IBackChannelAuthenticationRequestStore backChannelAuthenticationRequestStore,
        IBackchannelAuthenticationUserNotificationService userLoginService,
        IClock clock,
        ILogger<BackchannelAuthenticationResponseGenerator> logger)
    {
        Options = options;
        BackChannelAuthenticationRequestStore = backChannelAuthenticationRequestStore;
        UserLoginService = userLoginService;
        Clock = clock;
        Logger = logger;
    }

    /// <inheritdoc/>
    public virtual async Task<BackchannelAuthenticationResponse> ProcessAsync(BackchannelAuthenticationRequestValidationResult validationResult)
    {
        using var activity = Tracing.BasicActivitySource.StartActivity("BackchannelAuthenticationResponseGenerator.Process");

        if (validationResult == null) throw new ArgumentNullException(nameof(validationResult));
        if (validationResult.ValidatedRequest == null) throw new ArgumentNullException(nameof(validationResult.ValidatedRequest));
        if (validationResult.ValidatedRequest.Client == null) throw new ArgumentNullException(nameof(validationResult.ValidatedRequest.Client));

        Logger.LogTrace("Creating response for backchannel authentication request");

        var request = new BackChannelAuthenticationRequest
        {
            CreationTime = Clock.UtcNow.UtcDateTime,
            ClientId = validationResult.ValidatedRequest.ClientId,
            RequestedScopes = validationResult.ValidatedRequest.ValidatedResources.RawScopeValues,
            RequestedResourceIndicators = validationResult.ValidatedRequest.RequestedResourceIndicators,
            Subject = validationResult.ValidatedRequest.Subject,
            Lifetime = validationResult.ValidatedRequest.Expiry,
            AuthenticationContextReferenceClasses = validationResult.ValidatedRequest.AuthenticationContextReferenceClasses,
            Tenant = validationResult.ValidatedRequest.Tenant,
            IdP = validationResult.ValidatedRequest.IdP,
            BindingMessage = validationResult.ValidatedRequest.BindingMessage,
            Properties = validationResult.ValidatedRequest.Properties,
        };

        var requestId = await BackChannelAuthenticationRequestStore.CreateRequestAsync(request);

        var interval = validationResult.ValidatedRequest.Client.PollingInterval ?? Options.Ciba.DefaultPollingInterval;
        var response = new BackchannelAuthenticationResponse()
        {
            AuthenticationRequestId = requestId,
            ExpiresIn = request.Lifetime,
            Interval = interval,
        };

        await UserLoginService.SendLoginRequestAsync(new BackchannelUserLoginRequest
        {
            InternalId = request.InternalId,
            Subject = validationResult.ValidatedRequest.Subject,
            Client = validationResult.ValidatedRequest.Client,
            ValidatedResources = validationResult.ValidatedRequest.ValidatedResources,
            RequestedResourceIndicators = validationResult.ValidatedRequest.RequestedResourceIndicators,
            BindingMessage = validationResult.ValidatedRequest.BindingMessage,
            AuthenticationContextReferenceClasses = validationResult.ValidatedRequest.AuthenticationContextReferenceClasses,
            Tenant = validationResult.ValidatedRequest.Tenant,
            IdP = validationResult.ValidatedRequest.IdP,
            Properties = validationResult.ValidatedRequest.Properties,
        });

        return response;
    }
}
