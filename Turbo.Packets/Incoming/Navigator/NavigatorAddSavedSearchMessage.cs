using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Packets.Incoming.Navigator
{
    public record NavigatorAddSavedSearchMessage : IMessageEvent
    {
        public string SearchCode { get; init; }
        public string Filter { get; init; }
    }
}
