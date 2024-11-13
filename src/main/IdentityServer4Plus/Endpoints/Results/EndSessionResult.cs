// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Configuration;
using IdentityServer4.Extensions;
using IdentityServer4.Hosting;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace IdentityServer4.Endpoints.Results;

/// <summary>
/// Result for endsession
/// </summary>
/// <seealso cref="IEndpointResult" />
public class EndSessionResult : EndpointResult<EndSessionResult>
{
    /// <summary>
    /// The result
    /// </summary>
    public EndSessionValidationResult Result { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EndSessionResult"/> class.
    /// </summary>
    /// <param name="result">The result.</param>
    /// <exception cref="System.ArgumentNullException">result</exception>
    public EndSessionResult(EndSessionValidationResult result)
    {
        Result = result ?? throw new ArgumentNullException(nameof(result));
    }
}


class EndSessionHttpWriter : IHttpResponseWriter<EndSessionResult>
{
    public EndSessionHttpWriter(
        IdentityServerOptions options,
        IClock clock,
        IServerUrls urls,
        IMessageStore<LogoutMessage> logoutMessageStore)
    {
        _options = options;
        _clock = clock;
        _urls = urls;
        _logoutMessageStore = logoutMessageStore;
    }

    private IdentityServerOptions _options;
    private IClock _clock;
    private IServerUrls _urls;
    private IMessageStore<LogoutMessage> _logoutMessageStore;

    public async Task WriteHttpResponse(EndSessionResult result, HttpContext context)
    {
        var validatedRequest = result.Result.IsError ? null : result.Result.ValidatedRequest;

        string id = null;

        if (validatedRequest != null)
        {
            var logoutMessage = new LogoutMessage(validatedRequest);
            if (logoutMessage.ContainsPayload)
            {
                var msg = new Message<LogoutMessage>(logoutMessage, _clock.UtcNow.UtcDateTime);
                id = await _logoutMessageStore.WriteAsync(msg);
            }
        }

        var redirect = _options.UserInteraction.LogoutUrl;

        if (redirect.IsLocalUrl())
        {
            redirect = _urls.GetIdentityServerRelativeUrl(redirect);
        }

        if (id != null)
        {
            redirect = redirect.AddQueryString(_options.UserInteraction.LogoutIdParameter, id);
        }

        context.Response.Redirect(redirect);
    }
}
