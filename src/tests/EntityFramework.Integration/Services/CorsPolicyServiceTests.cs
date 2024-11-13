// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Options;
using IdentityServer4.EntityFramework.Services;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EntityFramework.IntegrationTests.Services;


public class CorsPolicyServiceTests : IntegrationTest<CorsPolicyServiceTests, ConfigurationDbContext, ConfigurationStoreOptions>
{
    public CorsPolicyServiceTests(DatabaseProviderFixture<ConfigurationDbContext> fixture) : base(fixture)
    {
        foreach (var options in TestDatabaseProviders.SelectMany(x => x.Select(y => (DbContextOptions<ConfigurationDbContext>) y)).ToList())
        {
            using (var context = new ConfigurationDbContext(options))
                context.Database.EnsureCreated();
        }
    }

    [Theory, MemberData(nameof(TestDatabaseProviders))]
    public async Task IsOriginAllowedAsync_WhenOriginIsAllowed_ExpectTrue(DbContextOptions<ConfigurationDbContext> options)
    {
        const string testCorsOrigin = "https://identityserver.io/";

        using (var context = new ConfigurationDbContext(options))
        {
            context.Clients.Add(new Client
            {
                ClientId = Guid.NewGuid().ToString(),
                ClientName = Guid.NewGuid().ToString(),
                AllowedCorsOrigins = new List<string> { "https://www.identityserver.com" }
            }.ToEntity());
            context.Clients.Add(new Client
            {
                ClientId = "2",
                ClientName = "2",
                AllowedCorsOrigins = new List<string> { "https://www.identityserver.com", testCorsOrigin }
            }.ToEntity());
            context.SaveChanges();
        }

        bool result;
        using (var context = new ConfigurationDbContext(options))
        {
            var service = new CorsPolicyService(context, FakeLogger<CorsPolicyService>.Create(), new NoneCancellationTokenProvider());
            result = await service.IsOriginAllowedAsync(testCorsOrigin);
        }

        Assert.True(result);
    }

    [Theory, MemberData(nameof(TestDatabaseProviders))]
    public async Task IsOriginAllowedAsync_WhenOriginIsNotAllowed_ExpectFalse(DbContextOptions<ConfigurationDbContext> options)
    {
        using (var context = new ConfigurationDbContext(options))
        {
            context.Clients.Add(new Client
            {
                ClientId = Guid.NewGuid().ToString(),
                ClientName = Guid.NewGuid().ToString(),
                AllowedCorsOrigins = new List<string> { "https://www.identityserver.com" }
            }.ToEntity());
            context.SaveChanges();
        }

        bool result;
        using (var context = new ConfigurationDbContext(options))
        {
            var service = new CorsPolicyService(context, FakeLogger<CorsPolicyService>.Create(), new NoneCancellationTokenProvider());
            result = await service.IsOriginAllowedAsync("InvalidOrigin");
        }

        Assert.False(result);
    }
}
