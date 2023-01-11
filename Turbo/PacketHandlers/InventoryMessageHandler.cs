using System;
using Turbo.Core.Game.Inventory;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Inventory.Furni;
using Turbo.Core.PacketHandlers;

namespace Turbo.Main.PacketHandlers
{
    public class InventoryMessageHandler : IInventoryMessageHandler
    {
        private readonly IPacketMessageHub _messageHub;
        private readonly IRoomManager _roomManager;

        public InventoryMessageHandler(
            IPacketMessageHub messageHub,
            IRoomManager roomManager)
        {
            _messageHub = messageHub;
            _roomManager = roomManager;

            _messageHub.Subscribe<RequestFurniInventoryMessage>(this, OnRequestFurniInventoryMessage);
            _messageHub.Subscribe<RequestFurniInventoryWhenNotInRoomMessage>(this, OnRequestFurniInventoryWhenNotInRoomMessage);
            _messageHub.Subscribe<RequestRoomPropertySetMessage>(this, OnRequestRoomPropertySetMessage);
        }

        protected virtual void OnRequestFurniInventoryMessage(RequestFurniInventoryMessage message, ISession session)
        {
            if (session.Player == null) return;

            IPlayerFurnitureInventory playerFurnitureInventory = session.Player.PlayerInventory.FurnitureInventory;

            if (playerFurnitureInventory != null) playerFurnitureInventory.SendFurnitureToSession(session);
        }

        protected virtual void OnRequestFurniInventoryWhenNotInRoomMessage(RequestFurniInventoryWhenNotInRoomMessage message, ISession session)
        {

        }

        protected virtual void OnRequestRoomPropertySetMessage(RequestRoomPropertySetMessage message, ISession session)
        {

        }
    }
}

