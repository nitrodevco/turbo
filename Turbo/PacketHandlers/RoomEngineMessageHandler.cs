using System.Collections.Generic;
using Turbo.Core.Game.Navigator;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Room.Engine;
using Turbo.Packets.Outgoing.Room.Engine;
using Turbo.Rooms.Object.Logic.Avatar;
using Turbo.Rooms.Utils;

namespace Turbo.Main.PacketHandlers
{
    public class RoomEngineMessageHandler : IRoomEngineMessageHandler
    {
        private readonly IPacketMessageHub _messageHub;
        private readonly IRoomManager _roomManager;
        private readonly INavigatorManager _navigatorManager;

        public RoomEngineMessageHandler(
            IPacketMessageHub messageHub,
            IRoomManager roomManager,
            INavigatorManager navigatorManager)
        {
            _messageHub = messageHub;
            _roomManager = roomManager;
            _navigatorManager = navigatorManager;

            _messageHub.Subscribe<GetRoomEntryDataMessage>(this, OnGetRoomEntryDataMessage);
            _messageHub.Subscribe<GetFurnitureAliasesMessage>(this, OnGetFurnitureAliasesMessage);
            _messageHub.Subscribe<MoveAvatarMessage>(this, OnMoveAvatarMessage);
        }

        private async void OnGetRoomEntryDataMessage(GetRoomEntryDataMessage message, ISession session)
        {
            if (session.Player == null) return;

            await _navigatorManager.ContinueEnteringRoom(session.Player);
        }

        private async void OnGetFurnitureAliasesMessage(GetFurnitureAliasesMessage message, ISession session)
        {
            if (session.Player == null) return;

            await session.Send(new FurnitureAliasesMessage
            {
                Aliases = new Dictionary<string, string>()
            });
        }

        private void OnMoveAvatarMessage(MoveAvatarMessage message, ISession session)
        {
            if (session.Player == null) return;

            IRoomObject roomObject = session.Player.RoomObject;

            if (roomObject == null) return;

            ((MovingAvatarLogic)roomObject.Logic).WalkTo(new Point(message.X, message.Y));
        }
    }
}
