using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Navigator;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Navigator;
using Turbo.Packets.Incoming.Room.Engine;
using Turbo.Rooms.Object.Logic.Avatar;
using Turbo.Rooms.Utils;

namespace Turbo.Navigator
{
    public class NavigatorMessageHandler : INavigatorMessageHandler
    {
        private readonly IPacketMessageHub _messageHub;

        public NavigatorMessageHandler(IPacketMessageHub messageHub)
        {
            _messageHub = messageHub;

            _messageHub.Subscribe<GetGuestRoomMessage>(this, OnGetGuestRoomMessage);
        }

        private void OnGetGuestRoomMessage(GetGuestRoomMessage message, ISession session)
        {
            Trace.WriteLine(message.RoomId);
            if (session.Player == null) return;

            Trace.WriteLine(message.RoomId);

            // navigator manger, player, roomid, password, skipstate
        }
    }
}
