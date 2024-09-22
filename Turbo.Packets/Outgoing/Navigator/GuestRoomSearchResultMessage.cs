using System.Collections.Generic;
using Turbo.Core.Game.Navigator;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Navigator;

public class GuestRoomSearchResultMessage : IComposer
{
    public int SearchType { get; init; }
    public string SearchParam { get; init; }
    public List <IRoom> Rooms { get; init; } = [];
    
    public IAdData? Ad { get; init; }
}