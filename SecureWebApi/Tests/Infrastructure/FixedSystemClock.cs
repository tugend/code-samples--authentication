using System;
using Microsoft.AspNetCore.Authentication;

namespace SecureWebApi.Tests.Infrastructure
{
    public class FixedSystemClock : ISystemClock
    {
        public DateTimeOffset UtcNow { get; private set; }

        public FixedSystemClock() => UtcNow = DateTimeOffset.UtcNow;

        public void SetTime(DateTimeOffset now) => UtcNow = now;
    }
}