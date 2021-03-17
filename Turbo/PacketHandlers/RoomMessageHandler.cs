using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Room.Avatar;
using Turbo.Packets.Incoming.Room.Engine;
using Turbo.Packets.Incoming.Room.Session;
using Turbo.Packets.Outgoing.Room.Engine;
using Turbo.Rooms.Object.Logic.Avatar;
using Turbo.Rooms.Utils;

namespace Turbo.Main.PacketHandlers
{
    public class RoomMessageHandler : IRoomMessageHandler
    {
        private readonly IPacketMessageHub _messageHub;
        private readonly IRoomManager _roomManager;
        private readonly INavigatorManager _navigatorManager;

        public RoomMessageHandler(
            IPacketMessageHub messageHub,
            IRoomManager roomManager,
            INavigatorManager navigatorManager)
        {
            _messageHub = messageHub;
            _roomManager = roomManager;
            _navigatorManager = navigatorManager;

            _messageHub.Subscribe<OpenFlatConnectionMessage>(this, OnOpenFlatConnectionMessage);
            _messageHub.Subscribe<GetFurnitureAliasesMessage>(this, OnGetFurnitureAliasesMessage);
            _messageHub.Subscribe<AvatarExpressionMessage>(this, OnAvatarExpressionMessage);
            _messageHub.Subscribe<DanceMessage>(this, OnDanceMessage);
            _messageHub.Subscribe<MoveAvatarMessage>(this, OnMoveAvatarMessage);
        }

        private async void OnOpenFlatConnectionMessage(OpenFlatConnectionMessage message, ISession session)
        {
            if (session.Player == null) return;

            await _navigatorManager.EnterRoom(session.Player, message.RoomId);
        }

        private async void OnGetFurnitureAliasesMessage(GetFurnitureAliasesMessage message, ISession session)
        {
            if (session.Player == null) return;

            await session.Send(new FurnitureAliasesMessage
            {
                Aliases = new Dictionary<string, string>()
            });
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

            if(roomObject.Logic is AvatarLogic avatarLogic)
            {
                avatarLogic.Dance((RoomObjectAvatarDanceType) message.Style);
            }
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
