using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Room.Avatar;
using Turbo.Rooms.Object.Logic.Avatar;

namespace Turbo.Main.PacketHandlers
{
    public class RoomAvatarMessageHandler : IRoomAvatarMessageHandler
    {
        private readonly IPacketMessageHub _messageHub;
        private readonly IRoomManager _roomManager;

        public RoomAvatarMessageHandler(
            IPacketMessageHub messageHub,
            IRoomManager roomManager)
        {
            _messageHub = messageHub;
            _roomManager = roomManager;

            _messageHub.Subscribe<AvatarExpressionMessage>(this, OnAvatarExpressionMessage);
            _messageHub.Subscribe<DanceMessage>(this, OnDanceMessage);
        }

        private void OnAvatarExpressionMessage(AvatarExpressionMessage message, ISession session)
        {
            if (session.Player == null) return;

            IRoomObject roomObject = session.Player.RoomObject;

            if (roomObject == null) return;
        }

        private void OnDanceMessage(DanceMessage message, ISession session)
        {
            if (session.Player == null) return;

            IRoomObject roomObject = session.Player.RoomObject;

            if (roomObject == null) return;

            if (roomObject.Logic is AvatarLogic avatarLogic)
            {
                avatarLogic.Dance((RoomObjectAvatarDanceType)message.Style);
            }
        }
    }
}
