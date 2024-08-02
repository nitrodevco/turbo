using Turbo.Core.Game.Navigator;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Room.Session;

namespace Turbo.Main.PacketHandlers;

public class RoomSessionMessageHandler(
    IPacketMessageHub messageHub,
    INavigatorManager navigatorManager)
    : IPacketHandlerManager
{
    public void Register()
    {
        messageHub.Subscribe<OpenFlatConnectionMessage>(this, OnOpenFlatConnectionMessage);
        messageHub.Subscribe<QuitMessage>(this, OnQuitMessage);
    }

    protected virtual async void OnOpenFlatConnectionMessage(OpenFlatConnectionMessage message, ISession session)
    {
        if (session.Player == null) return;

        await navigatorManager.EnterRoom(session.Player, message.RoomId);
    }

    protected virtual void OnQuitMessage(QuitMessage message, ISession session)
    {
        if (session.Player == null) return;

        session.Player.ClearRoomObject();
    }
}