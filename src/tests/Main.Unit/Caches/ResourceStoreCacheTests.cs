// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitTests.Services.Default.KeyManagement;
using Xunit;


namespace UnitTests.Caches;

public class ResourceStoreCacheTests
{
    List<Client> Clients { get; set; } = new List<Client>();
    List<IdentityResource> IdentityResources { get; set; } = new List<IdentityResource>();
    List<ApiResource> Resources { get; set; } = new List<ApiResource>();
    List<ApiScope> Scopes { get; set; } = new List<ApiScope>();

    MockClock _mockClock = new MockClock() { UtcNow = new DateTimeOffset(2022, 8, 9, 9, 0, 0, TimeSpan.Zero) };
    ServiceProvider _provider;

    public ResourceStoreCacheTests()
    {
        IdentityResources.Add(new IdentityResources.OpenId());
        IdentityResources.Add(new IdentityResources.Profile());
        
        Resources.Add(new ApiResource("urn:api1") { Scopes = { "scope1", "sharedscope1" } });
        Resources.Add(new ApiResource("urn:api2") { Scopes = { "scope2", "sharedscope1" } });

        Scopes.Add(new ApiScope("scope1"));
        Scopes.Add(new ApiScope("scope2"));
        Scopes.Add(new ApiScope("sharedscope1"));

        var services = new ServiceCollection();
        services.AddIdentityServer()
            .AddInMemoryClients(Clients)
            .AddInMemoryIdentityResources(IdentityResources)
            .AddInMemoryApiResources(Resources)
            .AddInMemoryApiScopes(Scopes)
            .AddResourceStoreCache<InMemoryResourcesStore>();

        services.AddSingleton(typeof(MockCache<>));
        services.AddSingleton(typeof(ICache<>), typeof(MockCache<>));
        services.AddSingleton<IClock>(_mockClock);
        
        _provider = services.BuildServiceProvider();
    }

    [Fact]
    public async Task FindIdentityResourcesByScopeNameAsync_should_populate_cache()
    {
        var cache = (MockCache<IdentityResource>) _provider.GetRequiredService<ICache<IdentityResource>>();
        var store = _provider.GetRequiredService<IResourceStore>();
        cache.CacheItems.Count.Should().Be(0);

        var results = await store.FindIdentityResourcesByScopeNameAsync(new[] { "profile" });

        cache.CacheItems.Count.Should().Be(1);
        cache.CacheItems.First().Value.Value.Name.Should().Be("profile");
    }

    [Fact]
    public async Task FindApiResourcesByScopeNameAsync_should_populate_cache()
    {
        var cache = (MockCache<CachingResourceStore<InMemoryResourcesStore>.ApiResourceNames>)
            _provider.GetRequiredService<ICache<CachingResourceStore<InMemoryResourcesStore>.ApiResourceNames>>();
        var store = _provider.GetRequiredService<IResourceStore>();
        cache.CacheItems.Count.Should().Be(0);

        var results = await store.FindApiResourcesByScopeNameAsync(new[] { "scope1" });

        cache.CacheItems.Count.Should().Be(1);
        cache.CacheItems.First().Value.Value.Names.Single().Should().Be("urn:api1");
    }

    [Fact]
    public async Task FindApiScopesByNameAsync_should_populate_cache()
    {
        var cache = (MockCache<ApiScope>) _provider.GetRequiredService<ICache<ApiScope>>();
        var store = _provider.GetRequiredService<IResourceStore>();
        cache.CacheItems.Count.Should().Be(0);

        var results = await store.FindApiScopesByNameAsync(new[] { "scope1" });

        cache.CacheItems.Count.Should().Be(1);
        cache.CacheItems.First().Value.Value.Name.Should().Be("scope1");
    }
}

