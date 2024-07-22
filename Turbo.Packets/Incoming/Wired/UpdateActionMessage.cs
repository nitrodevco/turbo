using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Wired;

public record UpdateActionMessage : UpdateWired, IMessageEvent
{
    public int Delay { get; init; }
}