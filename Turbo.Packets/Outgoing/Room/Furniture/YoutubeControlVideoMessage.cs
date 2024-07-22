using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Furniture;

public record YoutubeControlVideoMessage : IComposer
{
    public int ItemId { get; init; }
    public int CommandId { get; init; }
}