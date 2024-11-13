// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using FluentAssertions;
using IdentityServer4.Events;
using System;
using Xunit;

namespace UnitTests.Endpoints.Results;

public class EventTests
{

    [Fact]
    public void UnhandledExceptionEventCanCallToString()
    {
        try
        {
            throw new InvalidOperationException("Boom");
        }
        catch (Exception ex)
        {
            var unhandledExceptionEvent = new UnhandledExceptionEvent(ex);

            var s = unhandledExceptionEvent.ToString();

            s.Should().NotBeNullOrEmpty();
        }
    }
}
