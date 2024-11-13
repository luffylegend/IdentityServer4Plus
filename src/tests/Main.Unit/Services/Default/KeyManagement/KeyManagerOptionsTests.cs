// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using FluentAssertions;
using IdentityServer4.Configuration;
using System;
using Xunit;

namespace UnitTests.Services.Default.KeyManagement;

public class KeyManagerOptionsTests
{
    [Fact]
    public void InitializationSynchronizationDelay_should_be_greater_than_or_equal_to_zero()
    {
        var subject = new KeyManagementOptions
        {
            InitializationSynchronizationDelay = -TimeSpan.FromMinutes(1),
        };

        Action a = () => subject.Validate();
        a.Should().Throw<Exception>();
    }

    [Fact]
    public void InitializationDuration_should_be_greater_than_or_equal_to_zero()
    {
        var subject = new KeyManagementOptions
        {
            InitializationDuration = -TimeSpan.FromMinutes(1),
        };

        Action a = () => subject.Validate();
        a.Should().Throw<Exception>();
    }

    [Fact]
    public void InitializationKeyCacheDuration_should_be_greater_than_or_equal_to_zero()
    {
        var subject = new KeyManagementOptions
        {
            InitializationKeyCacheDuration = -TimeSpan.FromMinutes(1),
        };

        Action a = () => subject.Validate();
        a.Should().Throw<Exception>();
    }

    [Fact]
    public void Keycacheduration_should_be_greater_than_or_equal_to_zero()
    {
        var subject = new KeyManagementOptions
        {
            KeyCacheDuration = -TimeSpan.FromMinutes(1),
        };

        Action a = () => subject.Validate();
        a.Should().Throw<Exception>();
    }

    [Fact]
    public void Sctivation_should_be_greater_than_zero()
    {
        {
            var subject = new KeyManagementOptions
            {
                PropagationTime = TimeSpan.FromMinutes(0),
                RotationInterval = TimeSpan.FromMinutes(2),
                RetentionDuration = TimeSpan.FromMinutes(1)
            };

            Action a = () => subject.Validate();
            a.Should().Throw<Exception>();
        }
        {
            var subject = new KeyManagementOptions
            {
                PropagationTime = -TimeSpan.FromMinutes(1),
                RotationInterval = TimeSpan.FromMinutes(2),
                RetentionDuration = TimeSpan.FromMinutes(1)
            };

            Action a = () => subject.Validate();
            a.Should().Throw<Exception>();
        }
    }

    [Fact]
    public void Expiration_should_be_greater_than_zero()
    {
        {
            var subject = new KeyManagementOptions
            {
                PropagationTime = TimeSpan.FromMinutes(1),
                RotationInterval = TimeSpan.FromMinutes(0),
                RetentionDuration = TimeSpan.FromMinutes(3)
            };

            Action a = () => subject.Validate();
            a.Should().Throw<Exception>();
        }
        {
            var subject = new KeyManagementOptions
            {
                PropagationTime = TimeSpan.FromMinutes(1),
                RotationInterval = -TimeSpan.FromMinutes(1),
                RetentionDuration = TimeSpan.FromMinutes(2)
            };

            Action a = () => subject.Validate();
            a.Should().Throw<Exception>();
        }
    }

    [Fact]
    public void Retirement_should_be_greater_than_zero()
    {
        {
            var subject = new KeyManagementOptions
            {
                PropagationTime = TimeSpan.FromMinutes(1),
                RotationInterval = TimeSpan.FromMinutes(2),
                RetentionDuration = TimeSpan.FromMinutes(0)
            };

            Action a = () => subject.Validate();
            a.Should().Throw<Exception>();
        }
        {
            var subject = new KeyManagementOptions
            {
                PropagationTime = TimeSpan.FromMinutes(1),
                RotationInterval = TimeSpan.FromMinutes(2),
                RetentionDuration = -TimeSpan.FromMinutes(1)
            };

            Action a = () => subject.Validate();
            a.Should().Throw<Exception>();
        }
    }

    [Fact]
    public void Expiration_should_be_longer_than_activation_delay()
    {
        {
            var subject = new KeyManagementOptions
            {
                PropagationTime = TimeSpan.FromMinutes(1),
                RotationInterval = TimeSpan.FromMinutes(1),
                RetentionDuration = TimeSpan.FromMinutes(10)
            };

            Action a = () => subject.Validate();
            a.Should().Throw<Exception>();
        }

        {
            var subject = new KeyManagementOptions
            {
                PropagationTime = TimeSpan.FromMinutes(2),
                RotationInterval = TimeSpan.FromMinutes(1),
                RetentionDuration = TimeSpan.FromMinutes(10)
            };

            Action a = () => subject.Validate();
            a.Should().Throw<Exception>();
        }

        {
            var subject = new KeyManagementOptions
            {
                PropagationTime = TimeSpan.FromMinutes(1),
                RotationInterval = TimeSpan.FromMinutes(2),
                RetentionDuration = TimeSpan.FromMinutes(10)
            };

            Action a = () => subject.Validate();
            a.Should().NotThrow<Exception>();
        }
    }

    [Fact]
    public void Retirement_should_be_longer_than_expiration()
    {
        {
            var subject = new KeyManagementOptions
            {
                PropagationTime = TimeSpan.FromMinutes(1),
                RotationInterval = TimeSpan.FromMinutes(10),
                RetentionDuration = TimeSpan.FromMinutes(0),
            };

            Action a = () => subject.Validate();
            a.Should().Throw<Exception>();
        }

        {
            var subject = new KeyManagementOptions
            {
                PropagationTime = TimeSpan.FromMinutes(1),
                RotationInterval = TimeSpan.FromMinutes(10),
                RetentionDuration = -TimeSpan.FromMinutes(1),
            };

            Action a = () => subject.Validate();
            a.Should().Throw<Exception>();
        }

        {
            var subject = new KeyManagementOptions
            {
                PropagationTime = TimeSpan.FromMinutes(1),
                RotationInterval = TimeSpan.FromMinutes(10),
                RetentionDuration = TimeSpan.FromMinutes(20),
            };

            Action a = () => subject.Validate();
            a.Should().NotThrow<Exception>();
        }
    }
}
