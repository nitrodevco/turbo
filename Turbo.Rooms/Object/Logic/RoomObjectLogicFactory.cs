using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Turbo.Core.Game.Furniture.Data;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Rooms.Object.Attributes;
using Turbo.Rooms.Object.Logic.Avatar;
using Turbo.Rooms.Object.Logic.Furniture;
using Turbo.Rooms.Object.Logic.Furniture.Games.BattleBanzai;
using Turbo.Rooms.Object.Logic.Furniture.Games.BattleBanzai.Gates;
using Turbo.Rooms.Object.Logic.Furniture.Games.BattleBanzai.ScoreBoards;
using Turbo.Rooms.Object.Logic.Furniture.Wired.Conditions;
using Turbo.Rooms.Object.Logic.Furniture.Wired.Triggers;

namespace Turbo.Rooms.Object.Logic
{
    public class RoomObjectLogicFactory : IRoomObjectLogicFactory
    {
        public IDictionary<string, Type> Logics { get; private set; }
        
        public RoomObjectLogicFactory()
        {
            Logics = new Dictionary<string, Type>();

            foreach (var item in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsDefined(typeof(RoomObjectLogicAttribute))))
            {
                var attributeData = item.GetCustomAttribute<RoomObjectLogicAttribute>();

                if (attributeData == null) continue;
                
                Logics.TryAdd(attributeData.Name, item);
            }
        }

        public IRoomObjectLogic Create(string type)
        {
            var logicType = GetLogicType(type);

            if (logicType == null) return null;

            return (IRoomObjectLogic)Activator.CreateInstance(logicType);
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
