using System;

namespace Turbo.Core.Utilities;

public class TimeUtilities
{
    public static long GetCurrentMilliseconds()
    {
        return DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }
}