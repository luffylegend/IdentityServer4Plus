// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.Extensions.Logging;

namespace IdentityServer4.AccessToken.Tests;

public class AccessTokenHandlerTests 
{
    TestDPoPProofService _testDPoPProofService = new TestDPoPProofService();
    TestHttpMessageHandler _testHttpMessageHandler = new TestHttpMessageHandler();

    AccessTokenHandlerSubject _subject;

    public AccessTokenHandlerTests()
    {
        _subject = new AccessTokenHandlerSubject(_testDPoPProofService, new TestDPoPNonceStore(), new TestLoggerProvider().CreateLogger("AccessTokenHandlerSubject"));
        _subject.InnerHandler = _testHttpMessageHandler;
    }

    [Fact]
    public async Task Lower_case_token_type_should_be_converted_to_case_sensitive()
    {
        var client = new HttpClient(_subject);

        {
            _subject.AccessToken.AccessTokenType = "bearer";

            var response = await client.GetAsync("https://test/api");

            _testHttpMessageHandler.Request!.Headers.Authorization!.Scheme.ShouldBe("Bearer");
        }
    
        {
            _subject.AccessToken.AccessTokenType = "dpop";

            var response = await client.GetAsync("https://test/api");

            _testHttpMessageHandler.Request!.Headers.Authorization!.Scheme.ShouldBe("DPoP");
        }
    }

    public class TestHttpMessageHandler : HttpMessageHandler
    {
        public HttpRequestMessage? Request { get; set; }
        public HttpResponseMessage Response { get; set; } = new HttpResponseMessage(System.Net.HttpStatusCode.NoContent);

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Request = request;
            return Task.FromResult(Response);
        }
    }

    public class AccessTokenHandlerSubject : AccessTokenHandler
    {
        public ClientCredentialsToken AccessToken { get; set; } = new ClientCredentialsToken
        {
            AccessToken = "at",
            AccessTokenType = "bearer",
        };

        public AccessTokenHandlerSubject(IDPoPProofService dPoPProofService, IDPoPNonceStore dPoPNonceStore, ILogger logger) : base(dPoPProofService, dPoPNonceStore, logger)
        {
        }

        protected override Task<ClientCredentialsToken> GetAccessTokenAsync(bool forceRenewal, CancellationToken cancellationToken)
        {
            return Task.FromResult(AccessToken);
        }
    }
}
