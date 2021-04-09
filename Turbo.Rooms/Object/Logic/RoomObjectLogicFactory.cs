using System;
using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Rooms.Object.Logic.Avatar;
using Turbo.Rooms.Object.Logic.Furniture;
using Turbo.Rooms.Object.Logic.Furniture.Wired.Triggers;

namespace Turbo.Rooms.Object.Logic
{
    public class RoomObjectLogicFactory : IRoomObjectLogicFactory
    {
        public IDictionary<string, Type> Logics { get; private set; }


        public RoomObjectLogicFactory()
        {
            Logics = new Dictionary<string, Type>();

            Logics.Add(RoomObjectLogicType.User, typeof(AvatarLogic));
            Logics.Add(RoomObjectLogicType.Pet, typeof(PetLogic));
            Logics.Add(RoomObjectLogicType.Bot, typeof(BotLogic));
            Logics.Add(RoomObjectLogicType.RentableBot, typeof(RentableBotLogic));

            Logics.Add(RoomObjectLogicType.FurnitureDefault, typeof(FurnitureLogic));
            Logics.Add(RoomObjectLogicType.FurnitureStackHelper, typeof(FurnitureStackHelperLogic));
            Logics.Add(RoomObjectLogicType.FurnitureRoller, typeof(FurnitureRollerLogic));
            Logics.Add(RoomObjectLogicType.FurnitureGate, typeof(FurnitureGateLogic));
            Logics.Add(RoomObjectLogicType.FurnitureTeleport, typeof(FurnitureTeleportLogic));
            Logics.Add(RoomObjectLogicType.FurnitureDice, typeof(FurnitureDiceLogic));

            Logics.Add(RoomObjectLogicType.FurnitureWiredTriggerEnterRoom, typeof(FurnitureWiredTriggerEnterRoomLogic));
            Logics.Add(RoomObjectLogicType.FURNITURE_WIRED_TRIGGER_WALKS_ON_FURNI, typeof(FurnitureWiredTriggerWalksOnFurni));
        }

        public IRoomObjectLogic Create(string type)
        {
            if (!Logics.ContainsKey(type)) return null;

            return (IRoomObjectLogic)Activator.CreateInstance(Logics[type]);
        }
    }
}
