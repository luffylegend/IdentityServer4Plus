// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using FluentAssertions;
using IdentityServer4.Configuration;
using IdentityServer4.Events;
using System.Threading.Tasks;
using UnitTests.Common;
using UnitTests.Services.Default.KeyManagement;
using Xunit;

namespace UnitTests.Services.Default;

public class DefaultEventServiceTests
{
    [Fact]
    public async Task Raising_an_event_without_http_context_does_not_throw()
    {
        var options = new IdentityServerOptions();
        options.Events.RaiseInformationEvents = true;

        var sink = new MockEventSink();

        var sut = new DefaultEventService(
            options,
            // This is the most important part of this test. We want to ensure
            // that we don't throw exceptions when there is no http context available.
            new NullHttpContextAccessor(),
            sink,
            new MockClock());

        var evt = new TestEvent(id: 123);

        await sut.RaiseAsync(evt);

        sink.Events.Should().Contain(e => e.Id == 123);
    }
}

internal class TestEvent : Event
{
    public TestEvent(int id = 0, string message = "")
        : base(category: "Test", name: "Test", EventTypes.Information, id, message)
    {
    }
}
