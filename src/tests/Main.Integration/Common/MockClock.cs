using IdentityServer4;
using System;

namespace IdentityServer.IntegrationTests.Common
{
    class MockClock : IClock
    {
        public DateTimeOffset UtcNow { get; set; } = DateTimeOffset.UtcNow;
    }
}
