using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.FriendList;
public record MessengerInitMessage : IComposer
{
    public int userFriendLimit { get; init; }
    public int normalFriendLimit { get; init; }
    public int extendedFriendLimit { get; init; }
    //public List<IMessengerCategory> categories { get; init; }
}
