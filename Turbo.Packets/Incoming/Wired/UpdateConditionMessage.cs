using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Wired;

public record UpdateConditionMessage : UpdateWired, IMessageEvent
{
}