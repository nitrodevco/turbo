namespace Turbo.Packets.Incoming.Room.Avatar
{
    public record AvatarExpressionMessage : IMessageEvent
    {
        public int TypeCode { get; init; }
    }
}
