using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Events;

namespace Turbo.Events
{
    public class TurboEventListener : ITurboEventListener
    {
        public Delegate Action { get; set; }
        public WeakReference Sender { get; set; }
        public Type Type { get; set; }
    }
}