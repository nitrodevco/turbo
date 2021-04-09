using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Room.Furniture;
using Turbo.Rooms.Object.Logic.Furniture;

namespace Turbo.Main.PacketHandlers
{
    public class RoomFurnitureMessageHandler : IRoomFurnitureMessageHandler
    {
        private readonly IPacketMessageHub _messageHub;
        private readonly IRoomManager _roomManager;

        public RoomFurnitureMessageHandler(
            IPacketMessageHub messageHub,
            IRoomManager roomManager)
        {
            _messageHub = messageHub;
            _roomManager = roomManager;

            _messageHub.Subscribe<ThrowDiceMessage>(this, OnThrowDiceMessage);
            _messageHub.Subscribe<DiceOffMessage>(this, OnDiceOffMessage);
        }

        protected virtual void OnThrowDiceMessage(ThrowDiceMessage message, ISession session)
        {
            if (session.Player == null) return;

            IRoomObject roomObject = session.Player.RoomObject;

            if (roomObject == null) return;

            IRoomObject diceObject = roomObject.Room.RoomFurnitureManager.GetRoomObject(message.ObjectId);

            if (diceObject == null) return;

            if(diceObject.Logic is FurnitureDiceLogic diceLogic)
            {
                diceLogic.ThrowDice(roomObject);
            }
        }

        protected virtual void OnDiceOffMessage(DiceOffMessage message, ISession session)
        {
            if (session.Player == null) return;

            IRoomObject roomObject = session.Player.RoomObject;

            if (roomObject == null) return;

            IRoomObject diceObject = roomObject.Room.RoomFurnitureManager.GetRoomObject(message.ObjectId);

            if (diceObject == null) return;

            if (diceObject.Logic is FurnitureDiceLogic diceLogic)
            {
                diceLogic.DiceOff(roomObject);
            }
        }
    }
}
