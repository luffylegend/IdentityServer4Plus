// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Stores;
using System;
using Xunit;

namespace IdentityServer.UnitTests.Storage;

public class PersistedGrantFilterTests
{
    [Fact]
    public void Validate_should_not_throw_if_any_property_is_not_null_or_empty()
    {
        var filter = new PersistedGrantFilter { ClientId = "client-id" };
        filter.Validate();

        filter = new PersistedGrantFilter { ClientIds = new[] { "client-id" } };
        filter.Validate();

        filter = new PersistedGrantFilter { Type = IdentityServerConstants.PersistedGrantTypes.RefreshToken };
        filter.Validate();

        filter = new PersistedGrantFilter { Types = new[] { IdentityServerConstants.PersistedGrantTypes.RefreshToken } };
        filter.Validate();

        filter = new PersistedGrantFilter { SessionId = "sid" };
        filter.Validate();

        filter = new PersistedGrantFilter { SubjectId = "sub" };
        filter.Validate();

        filter = new PersistedGrantFilter 
        { 
            SessionId  = "sid",
            SubjectId = "sub",
            ClientId = "client-id",
            Type = IdentityServerConstants.PersistedGrantTypes.RefreshToken
        };
        filter.Validate();

        // Nothing to assert, test passes if Validate didn't throw
    }

    [Fact]
    public void Validate_should_throw_if_all_properties_are_null_or_empty()
    {
        var filter = new PersistedGrantFilter();
        Assert.Throws<ArgumentException>(() => filter.Validate());
    }
}

