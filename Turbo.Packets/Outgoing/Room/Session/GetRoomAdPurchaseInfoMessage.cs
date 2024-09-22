using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Session;

public record GetRoomAdPurchaseInfoMessage : IComposer
{
    public bool isHanditemControlBlocked { get; init; }
}