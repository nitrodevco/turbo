using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Packets.Incoming.Navigator
{
    public record NavigatorDeleteSavedSearchMessage : IMessageEvent
    {
        public int SearchID { get; init; }
    }
}
