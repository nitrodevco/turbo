using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Turbo.Core.Events
{
    public interface ITurboEventListener
    {
        public Delegate Action { get; set; }
        public WeakReference Sender { get; set; }
        public Type Type { get; set; }
    }
}