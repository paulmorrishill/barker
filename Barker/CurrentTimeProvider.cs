using System;

namespace Barker
{
    public interface CurrentTimeProvider
    {
        DateTime GetCurrentTime();
    }
}