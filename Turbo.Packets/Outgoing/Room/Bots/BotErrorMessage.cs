using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Bots;

public record BotErrorCode : IComposer
{
    public int ErrorCode { get; init; }
}