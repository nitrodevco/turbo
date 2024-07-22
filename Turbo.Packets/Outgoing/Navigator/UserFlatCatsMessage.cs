using System.Collections.Generic;
using Turbo.Core.Game.Navigator;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Navigator;

public record UserFlatCatsMessage : IComposer
{
    public List<INavigatorCategory> Categories { get; init; }
}