namespace Turbo.Packets.Incoming.Room.Engine
{
    public record CompostPlantMessage : IMessageEvent
    {
        public int PetId { get; init; }
    }
}
