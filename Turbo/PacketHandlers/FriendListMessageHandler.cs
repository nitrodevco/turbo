using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;

namespace Turbo.Main.PacketHandlers;

public class FriendListMessageHandler(IPacketMessageHub messageHub) : IPacketHandlerManager
{
    public void Register()
    {
        messageHub.Subscribe<Packets.Incoming.FriendList.MessengerInitMessage>(this, OnMessengerInitMessage);
    }

    protected virtual void OnMessengerInitMessage(Packets.Incoming.FriendList.MessengerInitMessage message, ISession session)
    {
        if (session.Player == null) return;

        session.Send(new Packets.Outgoing.FriendList.MessengerInitMessage
        {
            userFriendLimit = 500,
            normalFriendLimit = 500,
            extendedFriendLimit = 3000
            // categories = session.Player.FriendList.Categories
        });
    }
}
