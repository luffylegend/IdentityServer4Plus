// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using FluentAssertions;
using IdentityServer4.Configuration.Models.DynamicClientRegistration;
using IdentityServer4.Models;
using IntegrationTests.TestHosts;
using System.Net.Http.Json;
using Xunit;

namespace IntegrationTests;

public class DynamicClientRegistrationTests : ConfigurationIntegrationTestBase
{
    [Fact]
    public async Task Valid_request_creates_new_client()
    {
        IdentityServerHost.ApiScopes.Add(new ApiScope("api1"));

        var request = new DynamicClientRegistrationRequest
        {
            RedirectUris = new[] { new Uri("https://example.com/callback") },
            GrantTypes = new[] { "authorization_code" },
            ClientName = "test",
            ClientUri = new Uri("https://example.com"),
            DefaultMaxAge = 10000,
            Scope = "api1 openid profile"
        };
        var httpResponse = await ConfigurationHost.HttpClient!.PostAsJsonAsync("/connect/dcr", request);

        var response = await httpResponse.Content.ReadFromJsonAsync<DynamicClientRegistrationResponse>();
        response.Should().NotBeNull();
        var newClient = await IdentityServerHost.GetClientAsync(response!.ClientId); // Not null already asserted
        newClient.Should().NotBeNull();
        newClient.ClientId.Should().Be(response.ClientId);
        newClient.AllowedGrantTypes.Should().BeEquivalentTo(request.GrantTypes);
        newClient.ClientName.Should().Be(request.ClientName);
        newClient.ClientUri.Should().Be(request.ClientUri.ToString());
        newClient.UserSsoLifetime.Should().Be(request.DefaultMaxAge);
        newClient.ClientSecrets.Count.Should().Be(1);
        newClient.ClientSecrets.Single().Value.Should().Be(response.ClientSecret.Sha256());
    }
}
