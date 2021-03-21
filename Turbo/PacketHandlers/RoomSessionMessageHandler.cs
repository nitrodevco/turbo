using Turbo.Core.Game.Navigator;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Room.Session;

namespace Turbo.Main.PacketHandlers
{
    public class RoomSessionMessageHandler : IRoomSessionMessageHandler
    {
        private readonly IPacketMessageHub _messageHub;
        private readonly IRoomManager _roomManager;
        private readonly INavigatorManager _navigatorManager;

        public RoomSessionMessageHandler(
            IPacketMessageHub messageHub,
            IRoomManager roomManager,
            INavigatorManager navigatorManager)
        {
            _messageHub = messageHub;
            _roomManager = roomManager;
            _navigatorManager = navigatorManager;

            _messageHub.Subscribe<OpenFlatConnectionMessage>(this, OnOpenFlatConnectionMessage);
            _messageHub.Subscribe<QuitMessage>(this, OnQuitMessage);
        }

        private async void OnOpenFlatConnectionMessage(OpenFlatConnectionMessage message, ISession session)
        {
            if (session.Player == null) return;

            await _navigatorManager.EnterRoom(session.Player, message.RoomId);
        }

        private async void OnQuitMessage(QuitMessage message, ISession session)
        {
            if (session.Player == null) return;

            session.Player.ClearRoomObject();
        }
    }
}
