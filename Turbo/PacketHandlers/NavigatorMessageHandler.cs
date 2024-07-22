using Turbo.Core.Game.Navigator;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Navigator;

namespace Turbo.Main.PacketHandlers;

public class NavigatorMessageHandler : INavigatorMessageHandler
{
    private readonly IPacketMessageHub _messageHub;
    private readonly INavigatorManager _navigatorManager;

    public NavigatorMessageHandler(
        IPacketMessageHub messageHub,
        INavigatorManager navigatorManager)
    {
        _messageHub = messageHub;
        _navigatorManager = navigatorManager;

        _messageHub.Subscribe<GetGuestRoomMessage>(this, OnGetGuestRoomMessage);
        _messageHub.Subscribe<GetUserFlatCatsMessage>(this, OnGetUserFlatCatsMessage);
        _messageHub.Subscribe<NewNavigatorInitMessage>(this, OnNewNavigatorInitMessage);
    }

    protected virtual async void OnGetUserFlatCatsMessage(GetUserFlatCatsMessage message, ISession session)
    {
        if (session.Player == null) return;

        await _navigatorManager.SendNavigatorCategories(session.Player);
    }

    protected virtual async void OnGetGuestRoomMessage(GetGuestRoomMessage message, ISession session)
    {
        if (session.Player == null) return;

        await _navigatorManager.GetGuestRoomMessage(session.Player, message.RoomId, message.EnterRoom,
            message.RoomForward);
    }

    protected virtual async void OnNewNavigatorInitMessage(NewNavigatorInitMessage message, ISession session)
    {
        if (session.Player == null) return;

        await _navigatorManager.SendNavigatorSettings(session.Player);
        await _navigatorManager.SendNavigatorMetaData(session.Player);
        await _navigatorManager.SendNavigatorLiftedRooms(session.Player);
        await _navigatorManager.SendNavigatorSavedSearches(session.Player);
        await _navigatorManager.SendNavigatorEventCategories(session.Player);
    }
}