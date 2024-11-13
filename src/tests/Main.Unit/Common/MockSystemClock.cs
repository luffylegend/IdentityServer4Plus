using IdentityServer4;
using System;

namespace IdentityServer.UnitTests.Common
{
    class MockSystemClock : IClock
    {
        public DateTimeOffset Now { get; set; }

        public DateTimeOffset UtcNow
        {
            get
            {
                return Now;
            }
        }
    }
}
