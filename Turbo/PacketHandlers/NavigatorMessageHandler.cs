using Turbo.Core.Game.Navigator;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Navigator;

namespace Turbo.Main.PacketHandlers
{
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
            _messageHub.Subscribe<NewNavigatorInitMessage>(this, OnNewNavigatorInitMessage);
        }

        public async void OnGetGuestRoomMessage(GetGuestRoomMessage message, ISession session)
        {
            if (session.Player == null) return;

            await _navigatorManager.GetGuestRoomMessage(session.Player, message.RoomId, message.EnterRoom, message.RoomForward);
        }

        public async void OnNewNavigatorInitMessage(NewNavigatorInitMessage message, ISession session)
        {
            if (session.Player == null) return;

            await _navigatorManager.SendNavigatorMetaData(session.Player);
            await _navigatorManager.SendNavigatorLiftedRooms(session.Player);
            await _navigatorManager.SendNavigatorSavedSearches(session.Player);
        }
    }
}
