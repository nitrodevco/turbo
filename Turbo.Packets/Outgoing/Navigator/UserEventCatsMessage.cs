using System.Collections.Generic;
using Turbo.Core.Game.Navigator;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Navigator;

public class UserEventCatsMessage : IComposer
{
    public List<INavigatorEventCategory> EventCategories { get; init; }
}