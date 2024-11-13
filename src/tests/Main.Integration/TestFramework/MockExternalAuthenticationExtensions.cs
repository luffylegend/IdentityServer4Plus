// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.IntegrationTests.TestFramework;
using Microsoft.AspNetCore.Authentication;
using System;

namespace Microsoft.Extensions.DependencyInjection;

public static class MockExternalAuthenticationExtensions
{
    public static AuthenticationBuilder AddMockExternalAuthentication(this AuthenticationBuilder builder,
        string authenticationScheme = "external",
        string displayName = "external",
        Action<MockExternalAuthenticationOptions> configureOptions = null)
    {
        return builder.AddRemoteScheme<MockExternalAuthenticationOptions, MockExternalAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
    }
}