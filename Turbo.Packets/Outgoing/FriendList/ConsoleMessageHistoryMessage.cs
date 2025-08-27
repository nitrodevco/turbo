using System.Collections.Generic;
using Turbo.Core.Game.Messenger;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.FriendList;

public record ConsoleMessageHistoryMessage : IComposer
{
    public int ChatId { get; init; }
    public IReadOnlyList<IMessengerConsoleMessage> Messages { get; init; }
}