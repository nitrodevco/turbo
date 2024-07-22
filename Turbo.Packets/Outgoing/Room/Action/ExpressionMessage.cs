using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Action;

public record ExpressionMessage : IComposer
{
    public int ObjectId { get; init; }
    public int ExpressionType { get; init; }
}