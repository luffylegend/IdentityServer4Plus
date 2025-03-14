using FluentAssertions;
using IdentityServer.IntegrationTests.Common;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace IdentityServer.IntegrationTests.Endpoints.Token
{
    public class TokenEndpointTests
    {
        private const string Category = "Token endpoint";

        private string _client_id = "client";
        private string _client_secret = "secret";

        private string _scope_name = "api";
        private string _scope_secret = "api_secret";

        private IdentityServerPipeline _mockPipeline = new IdentityServerPipeline();

        public TokenEndpointTests()
        {
            _mockPipeline.Clients.Add(new Client
            {
                ClientId = _client_id,
                ClientSecrets = new List<Secret> { new Secret(_client_secret.Sha256()) },
                AllowedGrantTypes = { GrantType.ClientCredentials, GrantType.ResourceOwnerPassword },
                AllowedScopes = new List<string> { "api" },
            });


            _mockPipeline.Users.Add(new TestUser
            {
                SubjectId = "bob",
                Username = "bob",
                Password = "password",
                Claims = new Claim[]
                {
                    new Claim("name", "Bob Loblaw"),
                    new Claim("email", "bob@loblaw.com"),
                    new Claim("role", "Attorney")
                }
            });

            _mockPipeline.IdentityScopes.AddRange(new IdentityResource[] {
                new IdentityResources.OpenId()
            });

            _mockPipeline.ApiResources.AddRange(new ApiResource[] {
                new ApiResource
                {
                    Name = "api",
                    ApiSecrets = new List<Secret> { new Secret(_scope_secret.Sha256()) },
                    Scopes = {_scope_name}
                }
            });

            _mockPipeline.ApiScopes.AddRange(new[] {
                new ApiScope
                {
                    Name = _scope_name
                }
            });

            _mockPipeline.Initialize();
        }

        [Fact]
        [Trait("Category", Category)]
        public async Task Client_credentials_request_with_funny_headers_should_not_hang()
        {
            var data = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", _client_id },
                { "client_secret", _client_secret },
                { "scope", _scope_name },
            };
            var form = new FormUrlEncodedContent(data);
            _mockPipeline.BackChannelClient.DefaultRequestHeaders.Add("Referer", "http://127.0.0.1:33086/appservice/appservice?t=1564165664142?load");
            var response = await _mockPipeline.BackChannelClient.PostAsync(IdentityServerPipeline.TokenEndpoint, form);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonDocument.Parse(json).RootElement;
            bool containsError = result.TryGetProperty("error", out JsonElement errorElement);
            containsError.Should().BeFalse();
        }

        [Fact]
        [Trait("Category", Category)]
        public async Task Resource_owner_request_with_funny_headers_should_not_hang()
        {
            var data = new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "username", "bob" },
                { "password", "password" },
                { "client_id", _client_id },
                { "client_secret", _client_secret },
                { "scope", _scope_name },
            };
            var form = new FormUrlEncodedContent(data);
            _mockPipeline.BackChannelClient.DefaultRequestHeaders.Add("Referer", "http://127.0.0.1:33086/appservice/appservice?t=1564165664142?load");
            var response = await _mockPipeline.BackChannelClient.PostAsync(IdentityServerPipeline.TokenEndpoint, form);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonDocument.Parse(json).RootElement;
            bool containsError = result.TryGetProperty("error", out JsonElement errorElement);
            containsError.Should().BeFalse();
        }
    }
}
