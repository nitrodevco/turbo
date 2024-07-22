using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Furniture;

public record OpenMysteryTrophyMessage : IMessageEvent
{
    public int ObjectId { get; init; }
    public string TrophyInscription { get; init; }
}