// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;

namespace IdentityServer4.Validation;

internal interface IRequestObjectValidator
{
    Task<AuthorizeRequestValidationResult> LoadRequestObjectAsync(ValidatedAuthorizeRequest request);
    Task<AuthorizeRequestValidationResult> ValidatePushedAuthorizationRequest(ValidatedAuthorizeRequest request);
    Task<AuthorizeRequestValidationResult> ValidateRequestObjectAsync(ValidatedAuthorizeRequest request);
}
