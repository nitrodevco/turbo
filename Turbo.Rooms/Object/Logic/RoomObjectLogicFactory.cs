using System;
using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Rooms.Object.Logic.Avatar;
using Turbo.Rooms.Object.Logic.Furniture;

namespace Turbo.Rooms.Object.Logic
{
    public class RoomObjectLogicFactory : IRoomObjectLogicFactory
    {
        private readonly IDictionary<string, Type> _logics;

        public RoomObjectLogicFactory()
        {
            _logics = new Dictionary<string, Type>();

            _logics.Add("user", typeof(AvatarLogic));
            _logics.Add("pet", typeof(PetLogic));
            _logics.Add("bot", typeof(BotLogic));
            _logics.Add("rentablebot", typeof(RentableBotLogic));

            _logics.Add("default", typeof(FurnitureLogic));
            _logics.Add("stack_helper", typeof(FurnitureStackHelperLogic));
            _logics.Add("roller", typeof(FurnitureRollerLogic));
        }

        public IRoomObjectLogic GetLogic(string type)
        {
            Type logicType = _logics[type];

            if (logicType == null) return null;

            return (IRoomObjectLogic)Activator.CreateInstance(logicType);
        }
    }
}
