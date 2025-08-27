using Turbo.Core.Game.Messenger;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.FriendList;

public record NewConsoleMessageMessage : IComposer
{
    public required IMessengerConsoleMessage ConsoleMessage { get; init; }
}
