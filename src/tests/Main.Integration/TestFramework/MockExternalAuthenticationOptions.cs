// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Authentication;

namespace IdentityServer4.IntegrationTests.TestFramework;

public class MockExternalAuthenticationOptions : RemoteAuthenticationOptions
{
    public MockExternalAuthenticationOptions()
    {
        CallbackPath = "/external-callback";
    }
}
