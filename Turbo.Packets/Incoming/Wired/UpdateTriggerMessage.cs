using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Wired;

public record UpdateTriggerMessage : UpdateWired, IMessageEvent
{
}