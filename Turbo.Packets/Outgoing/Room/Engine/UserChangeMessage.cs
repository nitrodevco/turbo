using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Engine;

public record UserChangeMessage : IComposer
{
    public int Id { get; init; }
    public string Figure { get; init; }
    public string Sex { get; init; }
    public string CustomInfo { get; init; }
    public int ActivityPoints { get; init; }
}