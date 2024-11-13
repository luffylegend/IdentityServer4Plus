// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Test;
using IdentityServer4.Validation;
using IdentityModel;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection;


/// <summary>
/// Implementation of IBackchannelAuthenticationUserValidator using the test user store.
/// </summary>
public class TestBackchannelLoginUserValidator : IBackchannelAuthenticationUserValidator
{
    private readonly TestUserStore _testUserStore;

    /// <summary>
    /// Ctor
    /// </summary>
    public TestBackchannelLoginUserValidator(TestUserStore testUserStore)
    {
        _testUserStore = testUserStore;
    }

    /// <inheritdoc/>
    public Task<BackchannelAuthenticationUserValidationResult> ValidateRequestAsync(BackchannelAuthenticationUserValidatorContext userValidatorContext)
    {
        var result = new BackchannelAuthenticationUserValidationResult();

        TestUser user = default;

        if (userValidatorContext.LoginHint != null)
        {
            user = _testUserStore.FindByUsername(userValidatorContext.LoginHint);
        }
        else if (userValidatorContext.IdTokenHintClaims != null)
        {
            user = _testUserStore.FindBySubjectId(userValidatorContext.IdTokenHintClaims.SingleOrDefault(x => x.Type == JwtClaimTypes.Subject)?.Value);
        }

        if (user != null && user.IsActive)
        {
            var claims = new List<Claim>
        {
            new Claim(JwtClaimTypes.Subject, user.SubjectId)
        };
            var ci = new ClaimsIdentity(claims, "ciba");
            result.Subject = new ClaimsPrincipal(ci);
        }

        return Task.FromResult(result);
    }
}
