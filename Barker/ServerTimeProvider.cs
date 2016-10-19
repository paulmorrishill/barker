using System;

namespace Barker
{
    public class ServerTimeProvider : CurrentTimeProvider
    {
        public DateTime GetCurrentTime() => DateTime.Now;
    }
}