using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Navigator;

public record DoorbellMessage : IComposer
{
    public string Username { get; init; }
}