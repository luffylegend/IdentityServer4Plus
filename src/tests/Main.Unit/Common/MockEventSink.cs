// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Events;
using IdentityServer4.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTests.Common;

internal class MockEventSink : IEventSink
{
    public List<Event> Events { get; } = [];

    public Task PersistAsync(Event evt)
    {
        Events.Add(evt);
        return Task.CompletedTask;
    }
}
