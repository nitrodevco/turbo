namespace Turbo.Packets.Incoming.Room.Engine
{
    public record GetPetCommandsMessage : IMessageEvent
    {
        public int PetId { get; init; }
    }
}
