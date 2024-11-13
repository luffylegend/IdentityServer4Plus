// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using FluentAssertions;
using IdentityServer4.Configuration;
using IdentityServer4.Endpoints.Results;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnitTests.Common;
using Xunit;

namespace UnitTests.Endpoints.Results;

public class EndSessionCallbackResultTests
{
    private EndSessionCallbackHttpWriter _subject;

    private EndSessionCallbackValidationResult _result = new EndSessionCallbackValidationResult();
    private IdentityServerOptions _options = TestIdentityServerOptions.Create();

    private DefaultHttpContext _context = new DefaultHttpContext();

    public EndSessionCallbackResultTests()
    {
        _context.Request.Scheme = "https";
        _context.Request.Host = new HostString("server");
        _context.Response.Body = new MemoryStream();

        _subject = new EndSessionCallbackHttpWriter(_options);
    }

    [Fact]
    public async Task Error_should_return_400()
    {
        _result.IsError = true;

        await _subject.WriteHttpResponse(new EndSessionCallbackResult(_result), _context);

        _context.Response.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task Success_should_render_html_and_iframes()
    {
        _result.IsError = false;
        _result.FrontChannelLogoutUrls = new string[] { "http://foo.com", "http://bar.com" };

        await _subject.WriteHttpResponse(new EndSessionCallbackResult(_result), _context);

        _context.Response.ContentType.Should().StartWith("text/html");
        _context.Response.Headers["Cache-Control"].First().Should().Contain("no-store");
        _context.Response.Headers["Cache-Control"].First().Should().Contain("no-cache");
        _context.Response.Headers["Cache-Control"].First().Should().Contain("max-age=0");
        _context.Response.Headers["Content-Security-Policy"].First().Should().Contain("default-src 'none';");
        _context.Response.Headers["Content-Security-Policy"].First().Should().Contain("style-src 'sha256-e6FQZewefmod2S/5T11pTXjzE2vn3/8GRwWOs917YE4=';");
        _context.Response.Headers["Content-Security-Policy"].First().Should().Contain("frame-src http://foo.com http://bar.com");
        _context.Response.Headers["X-Content-Security-Policy"].First().Should().Contain("default-src 'none';");
        _context.Response.Headers["X-Content-Security-Policy"].First().Should().Contain("style-src 'sha256-e6FQZewefmod2S/5T11pTXjzE2vn3/8GRwWOs917YE4=';");
        _context.Response.Headers["X-Content-Security-Policy"].First().Should().Contain("frame-src http://foo.com http://bar.com");
        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        using (var rdr = new StreamReader(_context.Response.Body))
        {
            var html = rdr.ReadToEnd();
            html.Should().Contain("<iframe loading='eager' allow='' src='http://foo.com'></iframe>");
            html.Should().Contain("<iframe loading='eager' allow='' src='http://bar.com'></iframe>");
        }
    }

    [Fact]
    public async Task Fsuccess_should_add_unsafe_inline_for_csp_level_1()
    {
        _result.IsError = false;

        _options.Csp.Level = CspLevel.One;

        await _subject.WriteHttpResponse(new EndSessionCallbackResult(_result), _context);

        _context.Response.Headers["Content-Security-Policy"].First().Should().Contain("style-src 'unsafe-inline' 'sha256-e6FQZewefmod2S/5T11pTXjzE2vn3/8GRwWOs917YE4='");
        _context.Response.Headers["X-Content-Security-Policy"].First().Should().Contain("style-src 'unsafe-inline' 'sha256-e6FQZewefmod2S/5T11pTXjzE2vn3/8GRwWOs917YE4='");
    }

    [Fact]
    public async Task Form_post_mode_should_not_add_deprecated_header_when_it_is_disabled()
    {
        _result.IsError = false;

        _options.Csp.AddDeprecatedHeader = false;

        await _subject.WriteHttpResponse(new EndSessionCallbackResult(_result), _context);

        _context.Response.Headers["Content-Security-Policy"].First().Should().Contain("style-src 'sha256-e6FQZewefmod2S/5T11pTXjzE2vn3/8GRwWOs917YE4='");
        _context.Response.Headers["X-Content-Security-Policy"].Should().BeEmpty();
    }
}
