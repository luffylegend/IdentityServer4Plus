// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using FluentAssertions;
using IdentityServer4.Configuration;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Validation;

public class IdentityProviderConfigurationValidation
{
    private const string Category = "IdentityProvider Configuration Validation Tests";
    private IIdentityProviderConfigurationValidator _validator;
    IdentityServerOptions _options;

    public IdentityProviderConfigurationValidation()
    {
        _options = new IdentityServerOptions();
        _options.DynamicProviders.AddProviderType<OpenIdConnectHandler, OpenIdConnectOptions, OidcProvider>("oidc");

        _validator = new DefaultIdentityProviderConfigurationValidator(_options);
    }

    [Fact]
    [Trait("Category", Category)]
    public async Task Correctly_populated_idp_should_succeed()
    {
        var idp = new OidcProvider
        {
            Scheme = "scheme",
            ClientId = "client",
            ClientSecret = "secret",
            Authority = "authority",
            ResponseType = "code",
            Scope = "openid scope",
        };

        var ctx = new IdentityProviderConfigurationValidationContext(idp);
        await _validator.ValidateAsync(ctx);

        ctx.IsValid.Should().BeTrue();
    }

    [Fact]
    [Trait("Category", Category)]
    public async Task Missing_type_registration_should_fail()
    {
        var idp = new IdentityProvider("type")
        {
            Scheme = "scheme",
        };

        var ctx = new IdentityProviderConfigurationValidationContext(idp);
        await _validator.ValidateAsync(ctx);

        ctx.IsValid.Should().BeFalse();
        ctx.ErrorMessage.Should().Contain("registered");
    }

    [Fact]
    [Trait("Category", Category)]
    public async Task Custom_type_registration_should_succeed()
    {
        _options.DynamicProviders.AddProviderType<OpenIdConnectHandler, OpenIdConnectOptions, OidcProvider>("type");

        var idp = new IdentityProvider("type")
        {
            Scheme = "scheme"
        };

        var ctx = new IdentityProviderConfigurationValidationContext(idp);
        await _validator.ValidateAsync(ctx);

        ctx.IsValid.Should().BeTrue();
    }

    [Fact]
    [Trait("Category", Category)]
    public async Task Missing_scheme_should_fail()
    {
        var idp = new OidcProvider
        {
            ClientId = "client",
            ClientSecret = "secret",
            Authority = "authority",
            ResponseType = "code",
            Scope = "openid scope",
        };
        idp.Scheme = "";

        var ctx = new IdentityProviderConfigurationValidationContext(idp);
        await _validator.ValidateAsync(ctx);

        ctx.IsValid.Should().BeFalse();
        ctx.ErrorMessage.ToLowerInvariant().Should().Contain("scheme");
    }

    [Fact]
    [Trait("Category", Category)]
    public async Task Missing_clientid_should_fail()
    {
        var idp = new OidcProvider
        {
            Scheme = "scheme",
            ClientId = "",
            ClientSecret = "secret",
            Authority = "authority",
            ResponseType = "code",
            Scope = "openid scope",
        };

        var ctx = new IdentityProviderConfigurationValidationContext(idp);
        await _validator.ValidateAsync(ctx);

        ctx.IsValid.Should().BeFalse();
        ctx.ErrorMessage.ToLowerInvariant().Should().Contain("clientid");
    }

    [Fact]
    [Trait("Category", Category)]
    public async Task Missing_secret_should_be_allowed()
    {
        // we allow no secret because they might pull it from somewhere else
        var idp = new OidcProvider
        {
            Scheme = "scheme",
            ClientId = "client",
            ClientSecret = "",
            Authority = "authority",
            ResponseType = "code",
            Scope = "openid scope",
        };

        var ctx = new IdentityProviderConfigurationValidationContext(idp);
        await _validator.ValidateAsync(ctx);

        ctx.IsValid.Should().BeTrue();
    }

    [Fact]
    [Trait("Category", Category)]
    public async Task Missing_authority_should_fail()
    {
        var idp = new OidcProvider
        {
            Scheme = "scheme",
            ClientId = "client",
            ClientSecret = "secret",
            Authority = "",
            ResponseType = "code",
            Scope = "openid scope",
        };

        var ctx = new IdentityProviderConfigurationValidationContext(idp);
        await _validator.ValidateAsync(ctx);

        ctx.IsValid.Should().BeFalse();
        ctx.ErrorMessage.ToLowerInvariant().Should().Contain("authority");
    }

    [Fact]
    [Trait("Category", Category)]
    public async Task Missing_responsetype_should_fail()
    {
        var idp = new OidcProvider
        {
            Scheme = "scheme",
            ClientId = "client",
            ClientSecret = "secret",
            Authority = "authority",
            ResponseType = "",
            Scope = "openid scope",
        };

        var ctx = new IdentityProviderConfigurationValidationContext(idp);
        await _validator.ValidateAsync(ctx);

        ctx.IsValid.Should().BeFalse();
        ctx.ErrorMessage.ToLowerInvariant().Should().Contain("responsetype");
    }

    [Fact]
    [Trait("Category", Category)]
    public async Task Missing_scope_should_fail()
    {
        var idp = new OidcProvider
        {
            Scheme = "scheme",
            ClientId = "client",
            ClientSecret = "secret",
            Authority = "authority",
            ResponseType = "code",
            Scope = "",
        };

        var ctx = new IdentityProviderConfigurationValidationContext(idp);
        await _validator.ValidateAsync(ctx);

        ctx.IsValid.Should().BeFalse();
        ctx.ErrorMessage.ToLowerInvariant().Should().Contain("scope");
    }

    [Fact]
    [Trait("Category", Category)]
    public async Task When_implicit_flow_missing_clientid_should_succeed()
    {
        var idp = new OidcProvider
        {
            Scheme = "scheme",
            ClientId = "client",
            ClientSecret = "",
            Authority = "authority",
            ResponseType = "id_token",
            Scope = "openid scope",
        };

        var ctx = new IdentityProviderConfigurationValidationContext(idp);
        await _validator.ValidateAsync(ctx);

        ctx.IsValid.Should().BeTrue();
    }
}
