namespace Turbo.Packets.Incoming.Room.Avatar
{
    public record ChangePostureMessage : IMessageEvent
    {
        public int Posture { get; init; }
    }
}
