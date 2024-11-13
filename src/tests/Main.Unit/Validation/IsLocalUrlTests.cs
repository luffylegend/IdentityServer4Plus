// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Configuration;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using UnitTests.Common;
using UnitTests.Endpoints.Authorize;
using Xunit;

namespace UnitTests.Validation;

public class IsLocalUrlTests
{
    private const string QUERYPARAMETERS = "?client_id=mvc.code" +
        "&redirect_uri=https%3A%2F%2Flocalhost%3A44302%2Fsignin-oidc" +
        "&response_type=code" +
        "&scope=openid%20profile%20email%20custom.profile%20resource1.scope1%20resource2.scope1%20offline_access" +
        "&code_challenge=LcJN1shWmezC0J5EU7QOi7N_amBuvMDb6PcTY0sB2YY" +
        "&code_challenge_method=S256" +
        "&response_mode=form_post" +
        "&nonce=nonce" +
        "&state=state";

    public static IEnumerable<object[]> TestCases =>
        new List<object[]>
        {
        new object[] { "/connect/authorize/callback" + QUERYPARAMETERS, true },
        new object[] { "//evil.com/" + QUERYPARAMETERS, false },
        // Tab character
        new object[] { "/\t/evil.com/connect/authorize/callback" + QUERYPARAMETERS, false },
        // Tabs and Spaces
        new object[] { "/ \t/evil.com/connect/authorize/callback" + QUERYPARAMETERS, false },
        new object[] { "/  \t/evil.com/connect/authorize/callback" + QUERYPARAMETERS, false },
        new object[] { "/   \t/evil.com/connect/authorize/callback" + QUERYPARAMETERS, false },
        new object[] { "/\t /evil.com/connect/authorize/callback" + QUERYPARAMETERS, false },
        new object[] { "/\t  /evil.com/connect/authorize/callback" + QUERYPARAMETERS, false },
        new object[] { "/\t   /evil.com/connect/authorize/callback" + QUERYPARAMETERS, false },
        // Various new line related things
        new object[] { "/\n/evil.com/" + QUERYPARAMETERS, false },
        new object[] { "/\n\n/evil.com/" + QUERYPARAMETERS, false },
        new object[] { "/\r/evil.com/" + QUERYPARAMETERS, false },
        new object[] { "/\r\r/evil.com/" + QUERYPARAMETERS, false },
        new object[] { "/\r\n/evil.com/" + QUERYPARAMETERS, false },
        new object[] { "/\r\n\r\n/evil.com/" + QUERYPARAMETERS, false },
        // Tabs and Newlines
        new object[] { "/\t\n/evil.com/" + QUERYPARAMETERS, false },
        new object[] { "/\t\n\n/evil.com/" + QUERYPARAMETERS, false },
        new object[] { "/\t\r/evil.com/" + QUERYPARAMETERS, false },
        new object[] { "/\t\r\r/evil.com/" + QUERYPARAMETERS, false },
        new object[] { "/\t\r\n/evil.com/" + QUERYPARAMETERS, false },
        new object[] { "/\t\r\n\r\n/evil.com/" + QUERYPARAMETERS, false },
        new object[] { "/\n/evil.com\t/" + QUERYPARAMETERS, false },
        new object[] { "/\n\n/evil.com\t/" + QUERYPARAMETERS, false },
        new object[] { "/\r/evil.com\t/" + QUERYPARAMETERS, false },
        new object[] { "/\r\r/evil.com\t/" + QUERYPARAMETERS, false },
        new object[] { "/\r\n/evil.com\t/" + QUERYPARAMETERS, false },
        new object[] { "/\r\n\r\n/evil.com\t/" + QUERYPARAMETERS, false },
        };

    [Theory]
    [MemberData(nameof(TestCases))]
    public async Task GetAuthorizationContextAsync(string returnUrl, bool expected)
    {
        var interactionService = new DefaultIdentityServerInteractionService(null, null, null, null, null, null, null,
            GetReturnUrlParser(), new LoggerFactory().CreateLogger<DefaultIdentityServerInteractionService>());
        var actual = await interactionService.GetAuthorizationContextAsync(returnUrl);
        if (expected)
        {
            actual.Should().NotBeNull();
        }
        else
        {
            actual.Should().BeNull();
        }
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void IsLocalUrl(string returnUrl, bool expected)
    {
        returnUrl.IsLocalUrl().Should().Be(expected);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void GetIdentityServerRelativeUrl(string returnUrl, bool expected)
    {
        var serverUrls = new MockServerUrls
        {
            Origin = "https://localhost:5001",
            BasePath = "/"
        };
        var actual = serverUrls.GetIdentityServerRelativeUrl(returnUrl);
        if (expected)
        {
            actual.Should().NotBeNull();
        }
        else
        {
            actual.Should().BeNull();
        }
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public async Task OidcReturnUrlParser_ParseAsync(string returnUrl, bool expected)
    {
        var oidcParser = GetOidcReturnUrlParser();
        var actual = await oidcParser.ParseAsync(returnUrl);
        if (expected)
        {
            actual.Should().NotBeNull();
        }
        else
        {
            actual.Should().BeNull();
        }
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void OidcReturnUrlParser_IsValidReturnUrl(string returnUrl, bool expected)
    {
        var oidcParser = GetOidcReturnUrlParser();
        oidcParser.IsValidReturnUrl(returnUrl).Should().Be(expected);
    }


    [Theory]
    [MemberData(nameof(TestCases))]
    public void ReturnUrlParser_IsValidReturnUrl(string returnUrl, bool expected)
    {
        var parser = GetReturnUrlParser();
        parser.IsValidReturnUrl(returnUrl).Should().Be(expected);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public async Task ReturnUrlParser_ParseAsync(string returnUrl, bool expected)
    {
        var parser = GetReturnUrlParser();
        var actual = await parser.ParseAsync(returnUrl);
        if (expected)
        {
            actual.Should().NotBeNull();
        }
        else
        {
            actual.Should().BeNull();
        }
    }

    private static ReturnUrlParser GetReturnUrlParser()
    {
        var oidcParser = GetOidcReturnUrlParser();
        var parser = new ReturnUrlParser(new IReturnUrlParser[] { oidcParser });
        return parser;
    }

    private static OidcReturnUrlParser GetOidcReturnUrlParser()
    {
        return new OidcReturnUrlParser(
            new IdentityServerOptions(),
            new StubAuthorizeRequestValidator
            {
                Result = new AuthorizeRequestValidationResult
                (
                    new ValidatedAuthorizeRequest()
                )
            },
            new MockUserSession(),
            new MockServerUrls(),
            new LoggerFactory().CreateLogger<OidcReturnUrlParser>());
    }
}
