// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using FluentAssertions;
using IdentityModel;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using System;
using System.Collections.Specialized;
using UnitTests.Validation.Setup;
using Xunit;

namespace UnitTests.Validation.AuthorizeRequest_Validation;


public class Authorize_ProtocolValidation_Valid_PAR
{
    private const string Category = "AuthorizeRequest Protocol Validation - PAR";

    [Fact]
    [Trait("Category", Category)]
    public void Par_should_bind_client_to_pushed_request()
    {
        var initiallyPushedClientId = "clientId1";
        var par = new DeserializedPushedAuthorizationRequest
        {
            ReferenceValue = Guid.NewGuid().ToString(),
            ExpiresAtUtc = DateTime.UtcNow.AddMinutes(5),
            PushedParameters = new NameValueCollection
        {
            { OidcConstants.AuthorizeRequest.ClientId, initiallyPushedClientId }
        }
        };
        var differentClientInAuthorizeRequest = "notClientId1";
        var request = new ValidatedAuthorizeRequest
        {
            ClientId = differentClientInAuthorizeRequest
        };

        var validator = Factory.CreateRequestObjectValidator();
        var result = validator.ValidatePushedAuthorizationBindingToClient(par, request);

        result.Should().NotBeNull();
        result.IsError.Should().Be(true);
        result.ErrorDescription.Should().Be("invalid client for pushed authorization request");
    }

    [Fact]
    [Trait("Category", Category)]
    public void Expired_par_requests_should_fail()
    {
        var authorizeRequest = new ValidatedAuthorizeRequest();
        var par = new DeserializedPushedAuthorizationRequest
        {
            ReferenceValue = Guid.NewGuid().ToString(),
            ExpiresAtUtc = DateTime.UtcNow.AddSeconds(-1),
            PushedParameters = new NameValueCollection()
        };

        var validator = Factory.CreateRequestObjectValidator();
        var result = validator.ValidatePushedAuthorizationExpiration(par, authorizeRequest);

        result.Should().NotBeNull();
        result.IsError.Should().Be(true);
        result.ErrorDescription.Should().Be("expired pushed authorization request");
    }
}
