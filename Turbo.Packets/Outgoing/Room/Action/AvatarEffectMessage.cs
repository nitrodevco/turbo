using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Action
{
    public record AvatarEffectMessage : IComposer
    {
        public int UserId { get; init; }
        public int EffectId { get; init; }
        public int DelayMilliSeconds { get; init; }
    }
}
