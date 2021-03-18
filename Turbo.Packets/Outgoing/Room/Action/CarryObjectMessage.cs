using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Action
{
    public record CarryObjectMessage : IComposer
    {
        public int UserId { get; init; }
        public int ItemType { get; init; }
    }
}
