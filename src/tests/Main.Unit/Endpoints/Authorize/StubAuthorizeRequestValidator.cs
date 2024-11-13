// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Validation;
using System.Collections.Specialized;
using System.Security.Claims;
using System.Threading.Tasks;

namespace UnitTests.Endpoints.Authorize;

public class StubAuthorizeRequestValidator : IAuthorizeRequestValidator
{
    public AuthorizeRequestValidationResult Result { get; set; }

    public Task<AuthorizeRequestValidationResult> ValidateAsync(NameValueCollection parameters, ClaimsPrincipal subject = null, AuthorizeRequestType authorizeRequestType = AuthorizeRequestType.Authorize)
    {
        Result.ValidatedRequest.Raw = parameters;
        return Task.FromResult(Result);
    }
}
