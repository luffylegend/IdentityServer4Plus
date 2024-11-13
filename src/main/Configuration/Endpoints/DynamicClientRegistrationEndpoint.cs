// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Net.Http.Headers;
using System.Text.Json;
using IdentityServer4.Configuration.Models;
using IdentityServer4.Configuration.Models.DynamicClientRegistration;
using IdentityServer4.Configuration.RequestProcessing;
using IdentityServer4.Configuration.ResponseGeneration;
using IdentityServer4.Configuration.Validation.DynamicClientRegistration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IdentityServer4.Configuration;

/// <summary>
/// The dynamic client registration endpoint
/// </summary>
public class DynamicClientRegistrationEndpoint
{
    private readonly IDynamicClientRegistrationValidator _validator;
    private readonly IDynamicClientRegistrationRequestProcessor _processor;
    private readonly IDynamicClientRegistrationResponseGenerator _responseGenerator;
    private readonly ILogger<DynamicClientRegistrationEndpoint> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DynamicClientRegistrationEndpoint" /> class.
    /// </summary>
    public DynamicClientRegistrationEndpoint(
        IDynamicClientRegistrationValidator validator,
        IDynamicClientRegistrationRequestProcessor processor,
        IDynamicClientRegistrationResponseGenerator responseGenerator,
        ILogger<DynamicClientRegistrationEndpoint> logger)
    {
        _validator = validator;
        _logger = logger;
        _processor = processor;
        _responseGenerator = responseGenerator;
    }

    /// <summary>
    /// Processes requests to the dynamic client registration endpoint
    /// </summary>
    public async Task Process(HttpContext httpContext)
    {
        // Check content type
        if (!HasCorrectContentType(httpContext.Request))
        {
            await _responseGenerator.WriteContentTypeError(httpContext);
            return;
        }

        // Parse body
        var request = await TryParseAsync(httpContext.Request);
        if (request == null)
        {
            await _responseGenerator.WriteBadRequestError(httpContext);
            return;
        }

        var dcrContext = new DynamicClientRegistrationContext(request, httpContext.User);

        // Validate request values 
        var validationResult = await _validator.ValidateAsync(dcrContext);

        if (validationResult is DynamicClientRegistrationError validationError)
        {
            await _responseGenerator.WriteError(httpContext, validationError);
        }
        else
        {
            var processingResult = await _processor.ProcessAsync(dcrContext);
            if(processingResult is DynamicClientRegistrationError processingFailure)
            {
                await _responseGenerator.WriteError(httpContext, processingFailure);
            } 
            else if (processingResult is DynamicClientRegistrationResponse success)
            {
                await _responseGenerator.WriteSuccessResponse(httpContext, success);
            }
            else 
            {
                // This "can't happen" - if it does, something weird is going on.
                throw new InvalidOperationException("Results of request processing where neither success or failure");
            }
        }
    }

    private static bool HasCorrectContentType(HttpRequest request)
    {
        if (!MediaTypeHeaderValue.TryParse(request.ContentType, out var mt))
        {
            return false;
        }

        // Matches application/json
        if (mt.MediaType!.Equals("application/json", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return false;
    }

    private async Task<DynamicClientRegistrationRequest?> TryParseAsync(HttpRequest request)
    {
        try
        {
            var document = await request.ReadFromJsonAsync<DynamicClientRegistrationRequest>();
            if (document == null)
            {
                _logger.LogDebug("Dynamic client registration request body cannot be null");
            }
            return document;
        }
        catch (JsonException ex)
        {
            _logger.LogDebug(ex, "Failed to parse dynamic client registration request body");
            return default;
        }
    }
}
