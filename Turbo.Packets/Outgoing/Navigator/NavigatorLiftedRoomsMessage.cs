using System.Collections.Generic;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Navigator;

public record NavigatorLiftedRoomsMessage : IComposer
{
    public List<LiftedRoom> LiftedRooms { get; init; }
}

public record LiftedRoom
{
    public int FlatId { get; init; }
    public int Unused { get; init; }
    public string Image { get; init; }
    public string Caption { get; init; }
}