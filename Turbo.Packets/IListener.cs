using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Packets
{
    public interface IListener
    {
        public Delegate Action { get; set; }
        public WeakReference Sender { get; set; }
        public Type Type { get; set; }
    }
}
