using Turbo.Core.Game.Rooms;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Room.Furniture;
using Turbo.Rooms.Object.Logic.Furniture;

namespace Turbo.Main.PacketHandlers;

public class RoomFurnitureMessageHandler(IPacketMessageHub messageHub) : IPacketHandlerManager
{
    public void Register()
    {
        messageHub.Subscribe<ThrowDiceMessage>(this, OnThrowDiceMessage);
        messageHub.Subscribe<SetCustomStackingHeightMessage>(this, OnSetCustomStackingHeightMessage);
        messageHub.Subscribe<DiceOffMessage>(this, OnDiceOffMessage);
    }

    protected virtual void OnThrowDiceMessage(ThrowDiceMessage message, ISession session)
    {
        if (session.Player == null) return;

        var diceObject =
            session.Player.RoomObject?.Room.RoomFurnitureManager.FloorObjects.GetRoomObject(message.ObjectId);

        if (diceObject == null) return;

        if (diceObject.Logic is FurnitureDiceLogic diceLogic) diceLogic.ThrowDice(session.Player.RoomObject);
    }

    protected virtual void OnSetCustomStackingHeightMessage(SetCustomStackingHeightMessage message, ISession session)
    {
        if (session.Player == null) return;

        var stackHelperObject =
            session.Player.RoomObject?.Room.RoomFurnitureManager.FloorObjects.GetRoomObject(message.FurniId);

        if (stackHelperObject == null) return;

        if (stackHelperObject.Logic is FurnitureStackHelperLogic stackHelperLogic)
            stackHelperLogic.SetStackHelperHeight(session.Player.RoomObject, message.Height);
    }

    protected virtual void OnDiceOffMessage(DiceOffMessage message, ISession session)
    {
        if (session.Player == null) return;

        var diceObject =
            session.Player.RoomObject?.Room.RoomFurnitureManager.FloorObjects.GetRoomObject(message.ObjectId);

        if (diceObject == null) return;

        if (diceObject.Logic is FurnitureDiceLogic diceLogic) diceLogic.DiceOff(session.Player.RoomObject);
    }
}