using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Furniture;

public record YoutubeDisplayVideoMessage : IComposer
{
    public int FurniId { get; init; }
    public string VideoId { get; init; }
    public int StartAtSeconds { get; init; }
    public int EndAtSeconds { get; init; }
    public int State { get; init; }
}