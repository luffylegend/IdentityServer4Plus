// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using FluentAssertions;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using System;
using System.Collections.Generic;
using Xunit;

namespace UnitTests.Stores;

public class InMemoryClientStoreTests
{
    [Fact]
    public void InMemoryClient_should_throw_if_contain_duplicate_client_ids()
    {
        List<Client> clients = new List<Client>
    {
        new Client { ClientId = "1"},
        new Client { ClientId = "1"},
        new Client { ClientId = "3"}
    };

        Action act = () => new InMemoryClientStore(clients);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void InMemoryClient_should_not_throw_if_does_not_contain_duplicate_client_ids()
    {
        List<Client> clients = new List<Client>
    {
        new Client { ClientId = "1"},
        new Client { ClientId = "2"},
        new Client { ClientId = "3"}
    };

        new InMemoryClientStore(clients);
    }
}
