using System;
using System.Collections.Generic;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Core.Game.Rooms.Object.Logic.Wired;
using Turbo.Rooms.Object.Logic.Furniture.Wired.Actions;
using Turbo.Rooms.Object.Logic.Furniture.Wired.Conditions;

namespace Turbo.Rooms.Managers
{
    public class RoomWiredManager : IRoomWiredManager
    {
        private readonly IRoom _room;
        private readonly IRoomObjectLogicFactory _logicFactory;

        public RoomWiredManager(
            IRoom room,
            IRoomObjectLogicFactory logicFactory)
        {
            _room = room;
            _logicFactory = logicFactory;
        }

        public bool ProcessTriggers(string type, IWiredArguments wiredArguments = null)
        {
            if (!_logicFactory.Logics.TryGetValue(type, out Type logicType)) return false;

            IList<IRoomObject> roomObjects = _room.RoomFurnitureManager.GetRoomObjectsWithLogic(logicType);

            if (roomObjects.Count == 0) return false;

            bool didTrigger = false;

            foreach (IRoomObject roomObject in roomObjects)
            {
                if (!ProcessTrigger(roomObject, wiredArguments)) continue;

                didTrigger = true;
            }

            return didTrigger;
        }

        public bool ProcessTrigger(IRoomObject roomObject, IWiredArguments wiredArguments = null)
        {
            if (roomObject.Logic is not IFurnitureWiredLogic wiredLogic) return false;

            IRoomTile roomTile = wiredLogic.GetCurrentTile();

            if (roomTile == null) return false;

            if (!ProcessConditionsAtTile(roomTile, wiredArguments)) return false;

            if (!CanTrigger(roomObject, wiredArguments)) return false;

            ProcessActionsAtTile(roomTile, wiredArguments);

            return true;
        }

        public bool ProcessConditionsAtTile(IRoomTile roomTile, IWiredArguments wiredArguments = null)
        {
            IList<IRoomObject> roomObjects = GetConditionsAtTile(roomTile);

            if(roomObjects.Count > 0)
            {
                foreach(IRoomObject roomObject in roomObjects)
                {
                    if (!CanTrigger(roomObject, wiredArguments)) return false;
                }
            }

            return true;
        }

        public bool ProcessActionsAtTile(IRoomTile roomTile, IWiredArguments wiredArguments = null)
        {
            IList<IRoomObject> roomObjects = GetActionsAtTile(roomTile);

            if (roomObjects.Count > 0)
            {
                foreach (IRoomObject roomObject in roomObjects)
                {
                    if (!CanTrigger(roomObject, wiredArguments)) return false;
                }
            }

            return true;
        }

        public bool CanTrigger(IRoomObject roomObject, IWiredArguments wiredArguments = null)
        {
            IFurnitureWiredLogic wiredLogic = (IFurnitureWiredLogic)roomObject.Logic;

            if (!wiredLogic.CanTrigger(wiredArguments)) return false;

            wiredLogic.OnTriggered(wiredArguments);

            return true;
        }

        public IList<IRoomObject> GetActionsAtTile(IRoomTile roomTile)
        {
            IList<IRoomObject> roomObjects = new List<IRoomObject>();

            if (roomTile.Furniture.Count > 0)
            {
                foreach (IRoomObject roomObject in roomTile.Furniture.Values)
                {
                    if (roomObject.Logic is not FurnitureWiredActionLogic) continue;

                    roomObjects.Add(roomObject);
                }
            }

            return roomObjects;
        }

        public IList<IRoomObject> GetConditionsAtTile(IRoomTile roomTile)
        {
            IList<IRoomObject> roomObjects = new List<IRoomObject>();

            if(roomTile.Furniture.Count > 0)
            {
                foreach(IRoomObject roomObject in roomTile.Furniture.Values)
                {
                    if (roomObject.Logic is not FurnitureWiredConditionLogic) continue;

                    roomObjects.Add(roomObject);
                }
            }

            return roomObjects;
        }
    }
}
