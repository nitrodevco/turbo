using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Furniture;

public record SetYoutubeDisplayPlaylistMessage : IMessageEvent
{
    public int ObjectId { get; init; }
    public string SelectedItemName { get; init; }
}