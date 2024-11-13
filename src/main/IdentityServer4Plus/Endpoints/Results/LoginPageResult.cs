// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Configuration;
using IdentityServer4.Validation;

namespace IdentityServer4.Endpoints.Results;

/// <summary>
/// Result for login page
/// </summary>
public class LoginPageResult : AuthorizeInteractionPageResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LoginPageResult"/> class.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="options"></param>
    /// <exception cref="System.ArgumentNullException">request</exception>
    public LoginPageResult(ValidatedAuthorizeRequest request, IdentityServerOptions options)
        : base(request, options.UserInteraction.LoginUrl, options.UserInteraction.LoginReturnUrlParameter)
    {
    }
}
