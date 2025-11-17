// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using FluentAssertions;
using IdentityModel.Client;
using IdentityServer.IntegrationTests.Clients.Setup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace IdentityServer.IntegrationTests.Clients
{
    public class DiscoveryClientTests
    {
        private const string DiscoveryEndpoint = "https://server/.well-known/openid-configuration";

        private readonly HttpClient _client;

        public DiscoveryClientTests()
        {
            var builder = new HostBuilder()
                 .ConfigureWebHost(webHost =>
                 {
                     webHost.UseTestServer();
                     webHost.UseStartup<Startup>();
                 });
            var host = builder.Start();
            _client = host.GetTestClient();
        }

        [Fact]
        public async Task Discovery_document_should_have_expected_values()
        {
            var doc = await _client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = DiscoveryEndpoint,
                Policy =
                {
                    ValidateIssuerName = false
                }
            });

            // endpoints
            doc.TokenEndpoint.Should().Be("https://server/connect/token");
            doc.AuthorizeEndpoint.Should().Be("https://server/connect/authorize");
            doc.IntrospectionEndpoint.Should().Be("https://server/connect/introspect");
            doc.EndSessionEndpoint.Should().Be("https://server/connect/endsession");

            // jwk
            doc.KeySet.Keys.Count.Should().Be(1);
            doc.KeySet.Keys.First().E.Should().NotBeNull();
            doc.KeySet.Keys.First().N.Should().NotBeNull();
        }
    }
}