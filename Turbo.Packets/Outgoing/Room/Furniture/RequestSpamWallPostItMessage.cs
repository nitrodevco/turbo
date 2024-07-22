using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Furniture;

public record RequestSpamWallPostItMessage : IComposer
{
    public int ItemId { get; init; }
    public string Location { get; init; }
}