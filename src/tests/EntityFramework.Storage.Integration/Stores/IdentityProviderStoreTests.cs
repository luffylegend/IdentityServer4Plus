// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using FluentAssertions;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Options;
using IdentityServer4.EntityFramework.Stores;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EntityFramework.Storage.IntegrationTests.Stores;

public class IdentityProviderStoreTests : IntegrationTest<IdentityProviderStoreTests, ConfigurationDbContext, ConfigurationStoreOptions>
{
    public IdentityProviderStoreTests(DatabaseProviderFixture<ConfigurationDbContext> fixture) : base(fixture)
    {
        foreach (var options in TestDatabaseProviders.SelectMany(x => x.Select(y => (DbContextOptions<ConfigurationDbContext>) y)).ToList())
        {
            using (var context = new ConfigurationDbContext(options))
                context.Database.EnsureCreated();
        }
    }



    [Theory, MemberData(nameof(TestDatabaseProviders))]
    public async Task GetBySchemeAsync_should_find_by_scheme(DbContextOptions<ConfigurationDbContext> options)
    {
        using (var context = new ConfigurationDbContext(options))
        {
            var idp = new OidcProvider
            {
                Scheme = "scheme1", Type = "oidc"
            };
            context.IdentityProviders.Add(idp.ToEntity());
            context.SaveChanges();
        }

        using (var context = new ConfigurationDbContext(options))
        {
            var store = new IdentityProviderStore(context, FakeLogger<IdentityProviderStore>.Create(), new NoneCancellationTokenProvider());
            var item = await store.GetBySchemeAsync("scheme1");

            item.Should().NotBeNull();
        }
    }


    [Theory, MemberData(nameof(TestDatabaseProviders))]
    public async Task GetBySchemeAsync_should_filter_by_type(DbContextOptions<ConfigurationDbContext> options)
    {
        using (var context = new ConfigurationDbContext(options))
        {
            var idp = new OidcProvider
            {
                Scheme = "scheme2", Type = "saml"
            };
            context.IdentityProviders.Add(idp.ToEntity());
            context.SaveChanges();
        }

        using (var context = new ConfigurationDbContext(options))
        {
            var store = new IdentityProviderStore(context, FakeLogger<IdentityProviderStore>.Create(), new NoneCancellationTokenProvider());
            var item = await store.GetBySchemeAsync("scheme2");

            item.Should().BeNull();
        }
    }


    [Theory, MemberData(nameof(TestDatabaseProviders))]
    public async Task GetBySchemeAsync_should_filter_by_scheme_casing(DbContextOptions<ConfigurationDbContext> options)
    {
        using (var context = new ConfigurationDbContext(options))
        {
            var idp = new OidcProvider
            {
                Scheme = "SCHEME3", Type = "oidc"
            };
            context.IdentityProviders.Add(idp.ToEntity());
            context.SaveChanges();
        }

        using (var context = new ConfigurationDbContext(options))
        {
            var store = new IdentityProviderStore(context, FakeLogger<IdentityProviderStore>.Create(), new NoneCancellationTokenProvider());
            var item = await store.GetBySchemeAsync("scheme3");

            item.Should().BeNull();
        }
    }
}
