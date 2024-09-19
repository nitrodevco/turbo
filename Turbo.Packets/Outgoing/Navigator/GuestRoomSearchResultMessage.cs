using System.Collections.Generic;
using Turbo.Core.Game.Navigator;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Navigator;

public class GuestRoomSearchResultMessage : IComposer
{
    public int SearchType { get; init; }
    public string SearchParam { get; init; }
    public List <IRoomDetails> Rooms { get; init; } = new List<IRoomDetails>();
    
    public IAdData? Ad { get; init; }
}