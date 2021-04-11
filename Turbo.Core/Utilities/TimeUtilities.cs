using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Core.Utilities
{
    public class TimeUtilities
    {
        public static long GetCurrentMilliseconds()
        {
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
    }
}
