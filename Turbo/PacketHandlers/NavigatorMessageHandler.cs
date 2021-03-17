using Turbo.Core.Game.Navigator;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Navigator;
using Turbo.Packets.Incoming.Room.Engine;

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
            _messageHub.Subscribe<GetRoomEntryDataMessage>(this, OnGetRoomEntryDataMessage);
        }

        private async void OnGetGuestRoomMessage(GetGuestRoomMessage message, ISession session)
        {
            if (session.Player == null) return;

            await _navigatorManager.GetGuestRoomMessage(session.Player, message.RoomId, message.EnterRoom, message.RoomForward);
        }

        private async void OnGetRoomEntryDataMessage(GetRoomEntryDataMessage message, ISession session)
        {
            if (session.Player == null) return;

            await _navigatorManager.ContinueEnteringRoom(session.Player);
        }
    }
}
