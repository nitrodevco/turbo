using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Action;

public record AvatarEffectMessage : IComposer
{
    public int ObjectId { get; init; }
    public int EffectId { get; init; }
    public int DelayMilliSeconds { get; init; }
}