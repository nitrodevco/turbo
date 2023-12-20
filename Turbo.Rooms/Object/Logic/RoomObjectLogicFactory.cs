
using System;
using System.Collections.Generic;
using Turbo.Core.Events;
using Turbo.Core.Game.Furniture.Data;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Rooms.Object.Logic.Avatar;
using Turbo.Rooms.Object.Logic.Furniture;
using Turbo.Rooms.Object.Logic.Furniture.Games.BattleBanzai;
using Turbo.Rooms.Object.Logic.Furniture.Games.BattleBanzai.Gates;
using Turbo.Rooms.Object.Logic.Furniture.Games.BattleBanzai.ScoreBoards;

namespace Turbo.Rooms.Object.Logic
{
    public class RoomObjectLogicFactory : IRoomObjectLogicFactory
    {
        private readonly ITurboEventHub _eventHub;
        public IDictionary<string, Type> Logics { get; } = new Dictionary<string, Type>();


        public RoomObjectLogicFactory(ITurboEventHub eventHub)
        {
            _eventHub = eventHub;

            Logics.Add(RoomObjectLogicType.User, typeof(PlayerLogic));
            Logics.Add(RoomObjectLogicType.Pet, typeof(PetLogic));
            Logics.Add(RoomObjectLogicType.Bot, typeof(BotLogic));
            Logics.Add(RoomObjectLogicType.RentableBot, typeof(RentableBotLogic));

            Logics.Add(RoomObjectLogicType.FurnitureDefaultFloor, typeof(FurnitureFloorLogic));
            Logics.Add(RoomObjectLogicType.FurnitureDefaultWall, typeof(FurnitureWallLogic));
            Logics.Add(RoomObjectLogicType.FurnitureStackHelper, typeof(FurnitureStackHelperLogic));
            Logics.Add(RoomObjectLogicType.FurnitureRoller, typeof(FurnitureRollerLogic));
            Logics.Add(RoomObjectLogicType.FurnitureGate, typeof(FurnitureGateLogic));
            Logics.Add(RoomObjectLogicType.FurnitureTeleport, typeof(FurnitureTeleportLogic));
            Logics.Add(RoomObjectLogicType.FurnitureDice, typeof(FurnitureDiceLogic));

            #region Battle Banzai
            Logics.Add(RoomObjectLogicType.FurnitureBattleBanzaiTeleport, typeof(FurnitureBattleBanzaiTeleportLogic));
            Logics.Add(RoomObjectLogicType.FurnitureBattleBanzaiTile, typeof(FurnitureBattleBanzaiTileLogic));
            Logics.Add(RoomObjectLogicType.FurnitureBattleBanzaiTimer, typeof(FurnitureBattleBanzaiTimerLogic));
            Logics.Add(RoomObjectLogicType.FurnitureBattleBanzaiGateBlue, typeof(FurnitureBattleBanzaiGateBlueLogic));
            Logics.Add(RoomObjectLogicType.FurnitureBattleBanzaiGateGreen, typeof(FurnitureBattleBanzaiGateGreenLogic));
            Logics.Add(RoomObjectLogicType.FurnitureBattleBanzaiGateRed, typeof(FurnitureBattleBanzaiGateRedLogic));
            Logics.Add(RoomObjectLogicType.FurnitureBattleBanzaiGateYellow, typeof(FurnitureBattleBanzaiGateYellowLogic));
            Logics.Add(RoomObjectLogicType.FurnitureBattleBanzaiScoreboardBlue, typeof(FurnitureBattleBanzaiScoreboardBlueLogic));
            Logics.Add(RoomObjectLogicType.FurnitureBattleBanzaiScoreboardGreen, typeof(FurnitureBattleBanzaiScoreboardGreenLogic));
            Logics.Add(RoomObjectLogicType.FurnitureBattleBanzaiScoreboardRed, typeof(FurnitureBattleBanzaiScoreboardRedLogic));
            Logics.Add(RoomObjectLogicType.FurnitureBattleBanzaiScoreboardYellow, typeof(FurnitureBattleBanzaiScoreboardYellowLogic));
            #endregion
        }

        public IRoomObjectLogic Create(string type)
        {
            var logicType = GetLogicType(type);

            if (logicType == null) return null;

            var instance = (IRoomObjectLogic)Activator.CreateInstance(logicType);

            instance.SetEventHub(_eventHub);

            return instance;
        }

        public Type GetLogicType(string type)
        {
            if (!Logics.ContainsKey(type)) return null;

            return Logics[type];
        }

        public StuffDataKey GetStuffDataKeyForFurnitureType(string type)
        {
            if (!Logics.ContainsKey(type)) return StuffDataKey.LegacyKey;

            if (Logics[type].IsAssignableFrom(typeof(FurnitureLogicBase)))
            {
                //var logicType = typeof(FurnitureLogicBase) Logics[type];
            }

            return StuffDataKey.LegacyKey;
        }
    }
}
