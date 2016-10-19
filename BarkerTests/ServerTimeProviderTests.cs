using System;
using Barker;
using NUnit.Framework;
using Should;

namespace BarkerTests
{
    public class ServerTimeProviderTests
    {
        [Test]
        public void CanProvideTheServerTimeAsTheCurrentTime()
        {
            var serverTimeProvider = new ServerTimeProvider();
            var diffToServerTime = serverTimeProvider.GetCurrentTime() - DateTime.Now;
            diffToServerTime.Milliseconds.ShouldBeLessThan(100);
        }
    }
}