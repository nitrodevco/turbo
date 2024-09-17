using Turbo.Core.Game.Navigator;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Navigator;

namespace Turbo.Main.PacketHandlers;

public class NavigatorMessageHandler(
    IPacketMessageHub messageHub,
    INavigatorManager navigatorManager)
    : IPacketHandlerManager
{
    public void Register()
    {
        messageHub.Subscribe<GetUserFlatCatsMessage>(this, OnGetUserFlatCatsMessage);
        messageHub.Subscribe<GetGuestRoomMessage>(this, OnGetGuestRoomMessage);
        messageHub.Subscribe<NewNavigatorInitMessage>(this, OnNewNavigatorInitMessage);
    }

    protected virtual async void OnGetUserFlatCatsMessage(GetUserFlatCatsMessage message, ISession session)
    {
        if (session.Player == null) return;

        await navigatorManager.SendNavigatorCategories(session.Player);
    }

    protected virtual async void OnGetGuestRoomMessage(GetGuestRoomMessage message, ISession session)
    {
        if (session.Player == null) return;

        await navigatorManager.GetGuestRoomMessage(session.Player, message.RoomId, message.EnterRoom,
            message.RoomForward);
    }

    protected virtual async void OnNewNavigatorInitMessage(NewNavigatorInitMessage message, ISession session)
    {
        if (session.Player == null) return;

        await navigatorManager.SendNavigatorSettings(session.Player);
        await navigatorManager.SendNavigatorMetaData(session.Player);
        await navigatorManager.SendNavigatorLiftedRooms(session.Player);
        await navigatorManager.SendNavigatorSavedSearches(session.Player);
        await navigatorManager.SendNavigatorEventCategories(session.Player);
        await navigatorManager.SendNavigatorCollapsedCategories(session.Player);
    }
}