using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Navigator;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Navigator
{
    public record UserFlatCatsMessage : IComposer
    {
        public List<INavigatorCategory> Categories { get; init; }
    }
}
