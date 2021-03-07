using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Packets.Incoming.Navigator
{
    public record NavigatorSetSearchCodeViewModeMessage : IMessageEvent
    {
        public string CategoryName { get; init; }
        public int ViewMode { get; init; }
    }
}
