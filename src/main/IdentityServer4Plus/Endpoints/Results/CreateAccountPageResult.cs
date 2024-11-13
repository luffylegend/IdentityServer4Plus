// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Configuration;
using IdentityServer4.Validation;

namespace IdentityServer4.Endpoints.Results;

/// <summary>
/// Result for create account page
/// </summary>
public class CreateAccountPageResult : AuthorizeInteractionPageResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateAccountPageResult"/> class.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="options"></param>
    /// <exception cref="System.ArgumentNullException">request</exception>
    public CreateAccountPageResult(ValidatedAuthorizeRequest request, IdentityServerOptions options)
        : base(request, options.UserInteraction.CreateAccountUrl, options.UserInteraction.CreateAccountReturnUrlParameter)
    {
    }
}
