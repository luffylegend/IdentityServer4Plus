// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using FluentAssertions;
using IdentityServer.UnitTests.Common;
using IdentityServer4.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace IdentityServer.UnitTests.Services.Default
{
    public class DefaultCorsPolicyServiceTests
    {
        private const string Category = "DefaultCorsPolicyService";

        private DefaultCorsPolicyService _subject;

        public DefaultCorsPolicyServiceTests()
        {
            _subject = new DefaultCorsPolicyService(TestLogger.Create<DefaultCorsPolicyService>());
        }

        [Fact]
        [Trait("Category", Category)]
        public async Task IsOriginAllowed_null_param_ReturnsFalse()
        {
            (await _subject.IsOriginAllowedAsync(null)).Should().Be(false);
            (await _subject.IsOriginAllowedAsync(String.Empty)).Should().Be(false);
            (await _subject.IsOriginAllowedAsync("    ")).Should().Be(false);
        }

        [Fact]
        [Trait("Category", Category)]
        public async Task IsOriginAllowed_OriginIsAllowed_ReturnsTrue()
        {
            _subject.AllowedOrigins.Add("http://foo");
            (await _subject.IsOriginAllowedAsync("http://foo")).Should().Be(true);
        }

        [Fact]
        [Trait("Category", Category)]
        public async Task IsOriginAllowed_OriginIsNotAllowed_ReturnsFalse()
        {
            _subject.AllowedOrigins.Add("http://foo");
            (await _subject.IsOriginAllowedAsync("http://bar")).Should().Be(false);
        }

        [Fact]
        [Trait("Category", Category)]
        public async Task IsOriginAllowed_OriginIsInAllowedList_ReturnsTrue()
        {
            _subject.AllowedOrigins.Add("http://foo");
            _subject.AllowedOrigins.Add("http://bar");
            _subject.AllowedOrigins.Add("http://baz");
            (await _subject.IsOriginAllowedAsync("http://bar")).Should().Be(true);
        }

        [Fact]
        [Trait("Category", Category)]
        public async Task IsOriginAllowed_OriginIsNotInAllowedList_ReturnsFalse()
        {
            _subject.AllowedOrigins.Add("http://foo");
            _subject.AllowedOrigins.Add("http://bar");
            _subject.AllowedOrigins.Add("http://baz");
            (await _subject.IsOriginAllowedAsync("http://quux")).Should().Be(false);
        }

        [Fact]
        [Trait("Category", Category)]
        public async Task IsOriginAllowed_AllowAllTrue_ReturnsTrue()
        {
            _subject.AllowAll = true;
            (await _subject.IsOriginAllowedAsync("http://foo")).Should().Be(true);
        }
    }
}
