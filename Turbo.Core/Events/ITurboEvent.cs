using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Turbo.Core.Events
{
    public interface ITurboEvent
    {
        public bool IsCancelled { get; }

        public void Cancel();
    }
}