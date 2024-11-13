// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using FluentAssertions;
using IdentityServer4.Configuration;
using IdentityServer4.Endpoints.Results;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnitTests.Common;
using Xunit;

namespace UnitTests.Endpoints.Results;

public class EndSessionResultTests
{
    private EndSessionHttpWriter _subject;

    private EndSessionValidationResult _result = new EndSessionValidationResult();
    private IdentityServerOptions _options = new IdentityServerOptions();
    private MockMessageStore<LogoutMessage> _mockLogoutMessageStore = new MockMessageStore<LogoutMessage>();

    private DefaultServerUrls _urls;

    private DefaultHttpContext _context = new DefaultHttpContext();

    public EndSessionResultTests()
    {
        _urls = new DefaultServerUrls(new HttpContextAccessor { HttpContext = _context });

        _urls.Origin = "https://server";

        _options.UserInteraction.LogoutUrl = "~/logout";
        _options.UserInteraction.LogoutIdParameter = "logoutId";

        _subject = new EndSessionHttpWriter(_options, new StubClock(), _urls, _mockLogoutMessageStore);
    }

    [Fact]
    public async Task Validated_signout_should_pass_logout_message()
    {
        _result.IsError = false;
        _result.ValidatedRequest = new ValidatedEndSessionRequest
        {
            Client = new Client
            {
                ClientId = "client"
            },
            PostLogOutUri = "http://client/post-logout-callback"
        };

        await _subject.WriteHttpResponse(new EndSessionResult(_result), _context);

        _mockLogoutMessageStore.Messages.Count.Should().Be(1);
        var location = _context.Response.Headers["Location"].Single();
        var query = QueryHelpers.ParseQuery(new Uri(location).Query);

        location.Should().StartWith("https://server/logout");
        query["logoutId"].First().Should().Be(_mockLogoutMessageStore.Messages.First().Key);
    }

    [Fact]
    public async Task Unvalidated_signout_should_not_pass_logout_message()
    {
        _result.IsError = false;

        await _subject.WriteHttpResponse(new EndSessionResult(_result), _context);

        _mockLogoutMessageStore.Messages.Count.Should().Be(0);
        var location = _context.Response.Headers["Location"].Single();
        var query = QueryHelpers.ParseQuery(new Uri(location).Query);

        location.Should().StartWith("https://server/logout");
        query.Count.Should().Be(0);
    }

    [Fact]
    public async Task Error_result_should_not_pass_logout_message()
    {
        _result.IsError = true;
        _result.ValidatedRequest = new ValidatedEndSessionRequest
        {
            Client = new Client
            {
                ClientId = "client"
            },
            PostLogOutUri = "http://client/post-logout-callback"
        };

        await _subject.WriteHttpResponse(new EndSessionResult(_result), _context);

        _mockLogoutMessageStore.Messages.Count.Should().Be(0);
        var location = _context.Response.Headers["Location"].Single();
        var query = QueryHelpers.ParseQuery(new Uri(location).Query);

        location.Should().StartWith("https://server/logout");
        query.Count.Should().Be(0);
    }
}
