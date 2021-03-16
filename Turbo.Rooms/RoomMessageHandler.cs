using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Room.Engine;
using Turbo.Rooms.Object.Logic.Avatar;
using Turbo.Rooms.Utils;

namespace Turbo.Rooms
{
    public class RoomMessageHandler : IRoomMessageHandler
    {
        private readonly IPacketMessageHub _messageHub;

        public RoomMessageHandler(IPacketMessageHub messageHub)
        {
            _messageHub = messageHub;

            _messageHub.Subscribe<MoveAvatarMessage>(this, OnMoveAvatarMessage);
        }

        private void OnMoveAvatarMessage(MoveAvatarMessage message, ISession session)
        {
            if (session.Player == null) return;

            IRoomObject roomObject = session.Player.RoomObject;

            if (roomObject == null) return;

            ((MovingAvatarLogic) roomObject.Logic).WalkTo(new Point(message.X, message.Y));
        }
    }
}
