// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Test;
using System.Collections.Generic;

namespace IdentityServer.IntegrationTests.Endpoints.Introspection.Setup
{
    public static class Users
    {
        public static List<TestUser> Get()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "bob",
                    Password = "bob"
                }
            };
        }
    }
}