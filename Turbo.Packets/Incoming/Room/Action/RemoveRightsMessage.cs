namespace Turbo.Packets.Incoming.Room.Action
{
    public record RemoveRightsMessage : IMessageEvent
    {
        public int[] UserIds { get; init; }
    }
}
