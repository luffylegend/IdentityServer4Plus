// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using FluentAssertions;
using IdentityServer4;
using IdentityServer4.EntityFramework;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Options;
using IdentityServer4.EntityFramework.Stores;
using IdentityServer4.Stores;
using IdentityServer4.Test;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using IPersistedGrantStore = IdentityServer4.Stores.IPersistedGrantStore;

namespace EntityFramework.Storage.IntegrationTests.TokenCleanup;

public class TokenCleanupTests : IntegrationTest<TokenCleanupTests, PersistedGrantDbContext, OperationalStoreOptions>
{
    public TokenCleanupTests(DatabaseProviderFixture<PersistedGrantDbContext> fixture) : base(fixture)
    {
        foreach (var options in TestDatabaseProviders.SelectMany(x => x.Select(y => (DbContextOptions<PersistedGrantDbContext>) y)).ToList())
        {
            using (var context = new PersistedGrantDbContext(options))
            {
                context.Database.EnsureCreated();
            }

            // The db context is only created once, so before each test
            // we destroy any persisted grants that are left-over
            using (var context = new PersistedGrantDbContext(options))
            {
                var existing = context.PersistedGrants.ToArray();
                context.PersistedGrants.RemoveRange(existing);
                context.SaveChanges();
            }
        }
    }

    [Theory, MemberData(nameof(TestDatabaseProviders))]
    public async Task CleanupGrantsAsync_WhenExpiredGrantsExist_ExpectExpiredGrantsRemoved(DbContextOptions<PersistedGrantDbContext> options)
    {
        var expiredGrant = new PersistedGrant
        {
            Key = Guid.NewGuid().ToString(),
            ClientId = "app1",
            Type = "reference",
            SubjectId = "123",
            Expiration = DateTime.UtcNow.AddDays(-3),
            Data = "{!}"
        };

        using (var context = new PersistedGrantDbContext(options))
        {
            context.PersistedGrants.Add(expiredGrant);
            context.SaveChanges();
        }

        await CreateSut(options).CleanupGrantsAsync();

        using (var context = new PersistedGrantDbContext(options))
        {
            context.PersistedGrants.FirstOrDefault(x => x.Key == expiredGrant.Key).Should().BeNull();
        }
    }

    [Theory, MemberData(nameof(TestDatabaseProviders))]
    public async Task CleanupGrantsAsync_WhenValidGrantsExist_ExpectValidGrantsInDb(DbContextOptions<PersistedGrantDbContext> options)
    {
        var validGrant = new PersistedGrant
        {
            Key = Guid.NewGuid().ToString(),
            ClientId = "app1",
            Type = "reference",
            SubjectId = "123",
            Expiration = DateTime.UtcNow.AddDays(3),
            Data = "{!}"
        };

        using (var context = new PersistedGrantDbContext(options))
        {
            context.PersistedGrants.Add(validGrant);
            context.SaveChanges();
        }

        await CreateSut(options).CleanupGrantsAsync();

        using (var context = new PersistedGrantDbContext(options))
        {
            context.PersistedGrants.FirstOrDefault(x => x.Key == validGrant.Key).Should().NotBeNull();
        }
    }

    [Theory, MemberData(nameof(TestDatabaseProviders))]
    public async Task CleanupGrantsAsync_WhenBothExpiredAndValidGrantsExists_ExpectOnlyExpiredGrantsRemoved(DbContextOptions<PersistedGrantDbContext> options)
    {
        StoreOptions.TokenCleanupBatchSize = 20;

        var expiredGrants = Enumerable.Range(1, 45)
            .Select(i =>
                new PersistedGrant
                {
                    Key = "expired-" + i,
                    ClientId = "app1",
                    Type = "reference",
                    SubjectId = "123",
                    Expiration = DateTime.UtcNow.AddMinutes(-i),
                    Data = "{!}"
                });

        var validGrants = Enumerable.Range(1, 15)
            .Select(i =>
                new PersistedGrant
                {
                    Key = "valid-" + i,
                    ClientId = "app1",
                    Type = "reference",
                    SubjectId = "123",
                    Expiration = DateTime.UtcNow.AddMinutes(i),
                    Data = "{!}"
                });

        using (var context = new PersistedGrantDbContext(options))
        {
            context.PersistedGrants.AddRange(expiredGrants);
            context.PersistedGrants.AddRange(validGrants);
            context.SaveChanges();
        }

        await CreateSut(options).CleanupGrantsAsync();

        using (var context = new PersistedGrantDbContext(options))
        {
            var remaining = context.PersistedGrants.ToList();

            remaining.Count.Should().Be(15);
            remaining.All(r => r.Key.StartsWith("valid-")).Should().BeTrue();
        }
    }

    [Theory, MemberData(nameof(TestDatabaseProviders))]
    public async Task CleanupGrantsAsync_WhenExpiredDeviceGrantsExist_ExpectExpiredDeviceGrantsRemoved(DbContextOptions<PersistedGrantDbContext> options)
    {
        var expiredGrant = new DeviceFlowCodes
        {
            DeviceCode = Guid.NewGuid().ToString(),
            UserCode = Guid.NewGuid().ToString(),
            ClientId = "app1",
            SubjectId = "123",
            CreationTime = DateTime.UtcNow.AddDays(-4),
            Expiration = DateTime.UtcNow.AddDays(-3),
            Data = "{!}"
        };

        using (var context = new PersistedGrantDbContext(options))
        {
            context.DeviceFlowCodes.Add(expiredGrant);
            context.SaveChanges();
        }

        await CreateSut(options).CleanupGrantsAsync();

        using (var context = new PersistedGrantDbContext(options))
        {
            context.DeviceFlowCodes.FirstOrDefault(x => x.DeviceCode == expiredGrant.DeviceCode).Should().BeNull();
        }
    }

    [Theory, MemberData(nameof(TestDatabaseProviders))]
    public async Task CleanupGrantsAsync_WhenValidDeviceGrantsExist_ExpectValidDeviceGrantsInDb(DbContextOptions<PersistedGrantDbContext> options)
    {
        var validGrant = new DeviceFlowCodes
        {
            DeviceCode = Guid.NewGuid().ToString(),
            UserCode = "2468",
            ClientId = "app1",
            SubjectId = "123",
            CreationTime = DateTime.UtcNow.AddDays(-4),
            Expiration = DateTime.UtcNow.AddDays(3),
            Data = "{!}"
        };

        using (var context = new PersistedGrantDbContext(options))
        {
            context.DeviceFlowCodes.Add(validGrant);
            context.SaveChanges();
        }

        await CreateSut(options).CleanupGrantsAsync();

        using (var context = new PersistedGrantDbContext(options))
        {
            context.DeviceFlowCodes.FirstOrDefault(x => x.DeviceCode == validGrant.DeviceCode).Should().NotBeNull();
        }
    }


    [Theory, MemberData(nameof(TestDatabaseProviders))]
    public async Task CleanupGrantsAsync_WhenFlagIsOnAndConsumedGrantsExist_ExpectConsumedGrantsRemoved(DbContextOptions<PersistedGrantDbContext> options)
    {
        var consumedGrant = new PersistedGrant
        {
            Expiration = DateTime.UtcNow.AddDays(3), // Token not yet expired
            ConsumedTime = DateTime.UtcNow.AddMinutes(-15), // But was consumed

            Key = Guid.NewGuid().ToString(),
            Type = IdentityServerConstants.PersistedGrantTypes.RefreshToken,
            ClientId = "app1",
            Data = "{!}"
        };

        using (var context = new PersistedGrantDbContext(options))
        {
            context.PersistedGrants.Add(consumedGrant);
            context.SaveChanges();
        }

        await CreateSut(options, removeConsumedTokens: true).CleanupGrantsAsync();

        using (var context = new PersistedGrantDbContext(options))
        {
            context.PersistedGrants.FirstOrDefault(x => x.Id == consumedGrant.Id).Should().BeNull();
        }
    }

    [Theory, MemberData(nameof(TestDatabaseProviders))]
    public async Task CleanupGrantsAsync_WhenFlagIsOffAndConsumedGrantsExist_ExpectConsumedGrantsNotRemoved(DbContextOptions<PersistedGrantDbContext> options)
    {
        var consumedGrant = new PersistedGrant
        {
            Expiration = DateTime.UtcNow.AddDays(3), // Token not yet expired
            ConsumedTime = DateTime.UtcNow.AddMinutes(-15), // But was consumed

            Key = Guid.NewGuid().ToString(),
            Type = IdentityServerConstants.PersistedGrantTypes.RefreshToken,
            ClientId = "app1",
            Data = "{!}"
        };

        using (var context = new PersistedGrantDbContext(options))
        {
            context.PersistedGrants.Add(consumedGrant);
            context.SaveChanges();
        }

        await CreateSut(options, removeConsumedTokens: false).CleanupGrantsAsync();

        using (var context = new PersistedGrantDbContext(options))
        {
            context.PersistedGrants.FirstOrDefault(x => x.Id == consumedGrant.Id).Should().NotBeNull();
        }
    }

    [Theory, MemberData(nameof(TestDatabaseProviders))]
    public async Task CleanupGrantsAsync_ExpectBatchSizeIsRespected(DbContextOptions<PersistedGrantDbContext> options)
    {
        StoreOptions.TokenCleanupBatchSize = 20;

        var expectedPageCount = 5;

        using (var context = new PersistedGrantDbContext(options))
        {

            context.PersistedGrants.ToList().Should().BeEmpty();

            for (int i = 0; i < StoreOptions.TokenCleanupBatchSize * expectedPageCount; i++)
            {
                var expiredGrant = new PersistedGrant
                {
                    Expiration = DateTime.UtcNow.AddMinutes(-1),

                    Key = Guid.NewGuid().ToString(),
                    Type = IdentityServerConstants.PersistedGrantTypes.RefreshToken,
                    ClientId = "app1",
                    Data = "{!}"
                };
                context.PersistedGrants.Add(expiredGrant);
            }
            context.SaveChanges();

            context.PersistedGrants.Count().Should().Be(StoreOptions.TokenCleanupBatchSize * expectedPageCount);

        }

        var mockNotifications = new MockOperationalStoreNotification();

        await CreateSut(options, svcs =>
        {
            svcs.AddSingleton<IOperationalStoreNotification>(mockNotifications);
        }).CleanupGrantsAsync();

        // The right number of batches executed
        mockNotifications.PersistedGrantNotifications.Count.Should().Be(expectedPageCount);

        // Each batch contained the expected number of grants
        foreach (var notification in mockNotifications.PersistedGrantNotifications)
        {
            notification.Count().Should().Be(StoreOptions.TokenCleanupBatchSize);
        }

        // All grants are removed because they were all expired
        using (var context = new PersistedGrantDbContext(options))
        {
            context.PersistedGrants.ToList().Should().BeEmpty();
        }
    }

    [Theory, MemberData(nameof(TestDatabaseProviders))]
    public async Task CleanupGrantsAsync_InsertsBetweenBatches_ExpectAdditionalBatches(DbContextOptions<PersistedGrantDbContext> options)
    {
        StoreOptions.TokenCleanupBatchSize = 20;

        var expectedPageCount = 5;

        using (var context = new PersistedGrantDbContext(options))
        {
            for (int i = 0; i < StoreOptions.TokenCleanupBatchSize * expectedPageCount; i++)
            {
                var expiredGrant = new PersistedGrant
                {
                    Expiration = DateTime.UtcNow.AddMinutes(-1),

                    Key = Guid.NewGuid().ToString(),
                    Type = IdentityServerConstants.PersistedGrantTypes.RefreshToken,
                    ClientId = "app1",
                    Data = "{!}"
                };
                context.PersistedGrants.Add(expiredGrant);
            }
            context.SaveChanges();
        }

        // Whenever we cleanup a batch of grants, a new (expired) grant is inserted
        var mockNotifications = new MockOperationalStoreNotification()
        {
            OnPersistedGrantsRemoved = grants =>
            {
                using (var context = new PersistedGrantDbContext(options))
                {
                    var expiredGrant = new PersistedGrant
                    {
                        Expiration = DateTime.UtcNow.AddMinutes(-1),

                        Key = Guid.NewGuid().ToString(),
                        Type = IdentityServerConstants.PersistedGrantTypes.RefreshToken,
                        ClientId = "app1",
                        Data = "{Extra grant created between pages}"
                    };
                    context.PersistedGrants.Add(expiredGrant);
                    context.SaveChanges();
                }
            }
        };

        await CreateSut(options, svcs =>
        {
            svcs.AddSingleton<IOperationalStoreNotification>(mockNotifications);
        }).CleanupGrantsAsync();

        // Each batch created an extra grant, so we do an extra batch to clean up
        // the extras
        mockNotifications.PersistedGrantNotifications.Count.Should().Be(expectedPageCount + 1);

        // Each batch had the expected number of grants. Most batches had the batch size grants
        for (int i = 0; i < expectedPageCount; i++)
        {
            mockNotifications.PersistedGrantNotifications[i].Count().Should().Be(StoreOptions.TokenCleanupBatchSize);
        }

        // The last batch had the extras - there is one extra per page
        mockNotifications.PersistedGrantNotifications.Last().Count().Should().Be(expectedPageCount);

        // In the end, all but one get deleted
        // One final grant will be left behind, created by the last notification to fire
        // We can treat this as the first grant created after the job ran, 
        // we just are able to observe it because it was created in the final batch's notification
        using (var context = new PersistedGrantDbContext(options))
        {
            context.PersistedGrants.Count().Should().Be(1);
        }
    }

    [Theory, MemberData(nameof(TestDatabaseProviders))]
    public async Task CleanupGrantsAsync_WhenFlagIsOnAndConsumedGrantsExistAndDelayIsSet_ExpectConsumedGrantsRemovedRespectsDelay(DbContextOptions<PersistedGrantDbContext> options)
    {
        var delay = 100;

        // This grant was consumed long enough in the past that it should be deleted
        var oldConsumedGrant = new PersistedGrant
        {
            Expiration = DateTime.UtcNow.AddDays(3),                    // Token not yet expired
            ConsumedTime = DateTime.UtcNow.AddSeconds(-(delay + 100)),  // But was consumed MORE than the delay in the past

            Key = Guid.NewGuid().ToString(),
            Type = IdentityServerConstants.PersistedGrantTypes.RefreshToken,
            ClientId = "app1",
            Data = "{!}"
        };

        // This grant was consumed recently enough that it should not be deleted
        var newConsumedGrant = new PersistedGrant
        {
            Expiration = DateTime.UtcNow.AddDays(3),                    // Token not yet expired
            ConsumedTime = DateTime.UtcNow.AddSeconds(-(delay - 100)),  // But was consumed LESS than the delay in the past

            Key = Guid.NewGuid().ToString(),
            Type = IdentityServerConstants.PersistedGrantTypes.RefreshToken,
            ClientId = "app1",
            Data = "{!}"
        };

        using (var context = new PersistedGrantDbContext(options))
        {
            context.PersistedGrants.Add(newConsumedGrant);
            context.PersistedGrants.Add(oldConsumedGrant);
            context.SaveChanges();
        }

        await CreateSut(options, removeConsumedTokens: true, delay).CleanupGrantsAsync();

        using (var context = new PersistedGrantDbContext(options))
        {
            context.PersistedGrants.FirstOrDefault(x => x.Id == newConsumedGrant.Id).Should().NotBeNull();
            context.PersistedGrants.FirstOrDefault(x => x.Id == oldConsumedGrant.Id).Should().BeNull();
        }

    }


    private TokenCleanupService CreateSut(
        DbContextOptions<PersistedGrantDbContext> dbContextOpts,
        bool removeConsumedTokens,
        int consumedTokenCleanupDelay = 0
    )
    {
        StoreOptions.RemoveConsumedTokens = removeConsumedTokens;
        StoreOptions.ConsumedTokenCleanupDelay = consumedTokenCleanupDelay;
        return CreateSut(dbContextOpts);
    }

    private TokenCleanupService CreateSut(
        DbContextOptions<PersistedGrantDbContext> options,
        Action<IServiceCollection> configureServices
    )
    {
        IServiceCollection services = new ServiceCollection();

        configureServices(services);

        services.AddIdentityServer()
            .AddTestUsers(new List<TestUser>())
            .AddInMemoryClients(new List<IdentityServer4.Models.Client>())
            .AddInMemoryIdentityResources(new List<IdentityServer4.Models.IdentityResource>())
            .AddInMemoryApiResources(new List<IdentityServer4.Models.ApiResource>());

        services.AddScoped<IPersistedGrantDbContext, PersistedGrantDbContext>(_ =>
            new PersistedGrantDbContext(options));
        services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();
        services.AddTransient<IDeviceFlowStore, DeviceFlowStore>();

        services.AddTransient<ITokenCleanupService, TokenCleanupService>();
        services.AddSingleton(StoreOptions);

        return services.BuildServiceProvider().GetRequiredService<ITokenCleanupService>() as TokenCleanupService;
    }

    private TokenCleanupService CreateSut(DbContextOptions<PersistedGrantDbContext> options)
    {
        return CreateSut(options, _ => { });
    }
}
