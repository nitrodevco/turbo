using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Action
{
    public record UseObjectMessage : IComposer
    {
        public int UserId { get; init; }
        public int ItemType { get; init; }
    }
}
