// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using FluentAssertions;
using IdentityServer4.Configuration;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnitTests.Common;
using Xunit;

namespace UnitTests.Services.Default;

public class DistributedDeviceFlowThrottlingServiceTests
{
    private TestCache _cache = new TestCache();
    InMemoryClientStore _store;

    private readonly IdentityServerOptions _options = new IdentityServerOptions { DeviceFlow = new DeviceFlowOptions { Interval = 5 } };
    private readonly DeviceCode _deviceCode = new DeviceCode
    {
        Lifetime = 300,
        CreationTime = DateTime.UtcNow
    };

    private const string CacheKey = "devicecode_";
    private readonly DateTime _testDate = new DateTime(2018, 09, 01, 8, 0, 0, DateTimeKind.Utc);

    public DistributedDeviceFlowThrottlingServiceTests()
    {
        _store = new InMemoryClientStore(new List<Client>());
    }

    [Fact]
    public async Task First_Poll()
    {
        var handle = Guid.NewGuid().ToString();
        var service = new DistributedDeviceFlowThrottlingService(_cache, _store, new StubClock { UtcNowFunc = () => _testDate }, _options);

        var result = await service.ShouldSlowDown(handle, _deviceCode);

        result.Should().BeFalse();

        CheckCacheEntry(handle);
    }

    [Fact]
    public async Task Second_Poll_Too_Fast()
    {
        var handle = Guid.NewGuid().ToString();
        var service = new DistributedDeviceFlowThrottlingService(_cache, _store, new StubClock { UtcNowFunc = () => _testDate }, _options);

        _cache.Set(CacheKey + handle, Encoding.UTF8.GetBytes(_testDate.AddSeconds(-1).ToString("O")));

        var result = await service.ShouldSlowDown(handle, _deviceCode);

        result.Should().BeTrue();

        CheckCacheEntry(handle);
    }

    [Fact]
    public async Task Second_Poll_After_Interval()
    {
        var handle = Guid.NewGuid().ToString();

        var service = new DistributedDeviceFlowThrottlingService(_cache, _store, new StubClock { UtcNowFunc = () => _testDate }, _options);

        _cache.Set($"devicecode_{handle}", Encoding.UTF8.GetBytes(_testDate.AddSeconds(-_deviceCode.Lifetime - 1).ToString("O")));

        var result = await service.ShouldSlowDown(handle, _deviceCode);

        result.Should().BeFalse();

        CheckCacheEntry(handle);
    }

    /// <summary>
    /// Addresses race condition from #3860
    /// </summary>
    [Fact]
    public async Task Expired_Device_Code_Should_Not_Have_Expiry_in_Past()
    {
        var handle = Guid.NewGuid().ToString();
        _deviceCode.CreationTime = _testDate.AddSeconds(-_deviceCode.Lifetime * 2);

        var service = new DistributedDeviceFlowThrottlingService(_cache, _store, new StubClock { UtcNowFunc = () => _testDate }, _options);

        var result = await service.ShouldSlowDown(handle, _deviceCode);

        result.Should().BeFalse();

        _cache.Items.TryGetValue(CacheKey + handle, out var values).Should().BeTrue();
        values?.Item2.AbsoluteExpiration.Should().BeOnOrAfter(_testDate);
    }

    private void CheckCacheEntry(string handle)
    {
        _cache.Items.TryGetValue(CacheKey + handle, out var values).Should().BeTrue();

        var dateTimeAsString = Encoding.UTF8.GetString(values?.Item1);
        var dateTime = DateTime.Parse(dateTimeAsString).ToUniversalTime();
        dateTime.Should().Be(_testDate);

        values?.Item2.AbsoluteExpiration.Should().BeCloseTo(_testDate.AddSeconds(_deviceCode.Lifetime), TimeSpan.FromMicroseconds(1));
    }
}

internal class TestCache : IDistributedCache
{
    public readonly Dictionary<string, Tuple<byte[], DistributedCacheEntryOptions>> Items =
        new Dictionary<string, Tuple<byte[], DistributedCacheEntryOptions>>();

    public byte[] Get(string key)
    {
        if (Items.TryGetValue(key, out var value)) return value.Item1;
        return null;
    }

    public Task<byte[]> GetAsync(string key, CancellationToken token = new CancellationToken())
    {
        return Task.FromResult(Get(key));
    }

    public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
    {
        Items.Remove(key);

        Items.Add(key, new Tuple<byte[], DistributedCacheEntryOptions>(value, options));
    }

    public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = new CancellationToken())
    {
        Set(key, value, options);
        return Task.CompletedTask;
    }

    public void Refresh(string key)
    {
        throw new NotImplementedException();
    }

    public Task RefreshAsync(string key, CancellationToken token = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public void Remove(string key)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(string key, CancellationToken token = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}
