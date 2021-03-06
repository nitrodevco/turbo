namespace Turbo.Packets.Incoming.Room.Engine
{
    public record RemovePetFromFlatMessage : IMessageEvent
    {
        public int PetId { get; init; }
    }
}
