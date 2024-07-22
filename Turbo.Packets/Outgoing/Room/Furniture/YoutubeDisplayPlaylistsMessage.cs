using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Furniture;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Furniture;

public record YoutubeDisplayPlaylistsMessage : IComposer
{
    public int FurniId { get; init; }
    public List<YoutubePlaylist> Playlist { get; init; }
}