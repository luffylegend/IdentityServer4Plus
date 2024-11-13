// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using FluentAssertions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores.Default;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitTests.Common;
using Xunit;

namespace UnitTests.Stores.Default;

public class DistributedCacheAuthorizationParametersMessageStoreTests
{
    MockDistributedCache _mockCache = new MockDistributedCache();
    DistributedCacheAuthorizationParametersMessageStore _subject;
    public DistributedCacheAuthorizationParametersMessageStoreTests()
    {
        _subject = new DistributedCacheAuthorizationParametersMessageStore(_mockCache, new DefaultHandleGenerationService());
    }

    [Fact]
    public async Task DeleteAsync_should_remove_item()
    {
        _mockCache.Items.Count.Should().Be(0);

        var msg = new Message<IDictionary<string, string[]>>(new Dictionary<string, string[]>());
        var id = await _subject.WriteAsync(msg);

        _mockCache.Items.Count.Should().Be(1);

        await _subject.DeleteAsync(id);

        _mockCache.Items.Count.Should().Be(0);
    }
}
