// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Services;

namespace UnitTests.Common;

public class MockServerUrls : IServerUrls
{
    public string Origin { get; set; }
    public string BasePath { get; set; }
}
