using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Handshake
{
    public record OpenPetPackageResultMessage : IComposer
    {
        public int ObjectId { get; init; }
        public int NameValidationStatus { get; init; }
        public string NameValidationInfo { get; init; }
    }
}
