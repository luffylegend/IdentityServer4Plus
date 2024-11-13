// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using FluentAssertions;
using IdentityServer4.Configuration.Models;
using IdentityServer4.Configuration.Models.DynamicClientRegistration;
using IntegrationTests.TestHosts;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace IntegrationTests;

public class DynamicClientRegistrationValidationTests : ConfigurationIntegrationTestBase
{
    [Fact]
    public async Task Http_get_method_should_fail()
    {
        var response = await ConfigurationHost.HttpClient!.GetAsync("/connect/dcr");
        response.StatusCode.Should().Be(HttpStatusCode.MethodNotAllowed);
    }

    [Fact]
    public async Task Incorrect_content_type_should_fail()
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string> {
            { "redirect_uris", "https://example.com/callback" },
            { "grant_types", "authorization_code" }
        });
        var response = await ConfigurationHost.HttpClient!.PostAsync("/connect/dcr", content);
        response.StatusCode.Should().Be(HttpStatusCode.UnsupportedMediaType);
    }

    [Fact]
    public async Task Missing_grant_type_should_fail()
    {
        var response = await ConfigurationHost.HttpClient!.PostAsJsonAsync("/connect/dcr", new
        {
            redirect_uris = new[] { "https://example.com/callback" }
        });
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await response.Content.ReadFromJsonAsync<DynamicClientRegistrationError>();
        error?.Error.Should().Be("invalid_client_metadata");
    }

    [Fact]
    public async Task Unsupported_grant_type_should_fail()
    {
        var response = await ConfigurationHost.HttpClient!.PostAsJsonAsync("/connect/dcr", new
        {
            redirect_uris = new[] { "https://example.com/callback" },
            grant_types = new[] { "password" }
        });
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await response.Content.ReadFromJsonAsync<DynamicClientRegistrationError>();
        error?.Error.Should().Be("invalid_client_metadata");
    }

    [Fact]
    public async Task Client_credentials_with_redirect_uri_should_fail()
    {
        var response = await ConfigurationHost.HttpClient!.PostAsJsonAsync("/connect/dcr", new
        {
            redirect_uris = new[] { "https://example.com/callback" },
            grant_types = new[] { "client_credentials" }
        });
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await response.Content.ReadFromJsonAsync<DynamicClientRegistrationError>();
        error?.Error.Should().Be("invalid_redirect_uri");
    }

    [Fact]
    public async Task Auth_code_without_redirect_uri_should_fail()
    {
        var response = await ConfigurationHost.HttpClient!.PostAsJsonAsync("/connect/dcr", new
        {
            grant_types = new[] { "authorization_code", "client_credentials" }
        });
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await response.Content.ReadFromJsonAsync<DynamicClientRegistrationError>();
        error?.Error.Should().Be("invalid_redirect_uri");
    }

    [Fact]
    public async Task Client_credentials_and_refresh_token_should_fail()
    {
        var response = await ConfigurationHost.HttpClient!.PostAsJsonAsync("/connect/dcr", new
        {
            grant_types = new[] { "client_credentials", "refresh_token" }
        });
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await response.Content.ReadFromJsonAsync<DynamicClientRegistrationError>();
        error?.Error.Should().Be("invalid_client_metadata");
    }

    [Fact]
    public async Task Jwks_and_jwks_uri_used_together_should_fail()
    {
        var response = await ConfigurationHost.HttpClient!.PostAsJsonAsync("/connect/dcr",
            new DynamicClientRegistrationRequest
            {
                GrantTypes = { "client_credentials" },
                Jwks = new KeySet(Array.Empty<string>()),
                JwksUri = new Uri("https://example.com")
            }
        );
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await response.Content.ReadFromJsonAsync<DynamicClientRegistrationError>();
        error?.Error.Should().Be("invalid_client_metadata");
    }
}
