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

        public bool ProcessTriggers(string type)
        {
            Type logicType = _logicFactory.Logics[type];

            if (logicType == null) return false;

            IList<IRoomObject> roomObjects = _room.RoomFurnitureManager.GetRoomObjectsWithLogic(logicType);

            if (roomObjects.Count == 0) return false;

            bool didTrigger = false;

            foreach (IRoomObject roomObject in roomObjects)
            {
                if (!ProcessTrigger(roomObject)) continue;

                didTrigger = true;
            }

            return didTrigger;
        }

        public bool ProcessTrigger(IRoomObject roomObject)
        {
            if (roomObject.Logic is not IFurnitureWiredLogic wiredLogic) return false;

            IRoomTile roomTile = wiredLogic.GetCurrentTile();

            if (roomTile == null) return false;

            if (!ProcessConditionsAtTile(roomTile)) return false;

            if (!CanTrigger(roomObject)) return false;

            ProcessActionsAtTile(roomTile);

            return true;
        }

        public bool ProcessConditionsAtTile(IRoomTile roomTile)
        {
            IList<IRoomObject> roomObjects = GetConditionsAtTile(roomTile);

            if(roomObjects.Count > 0)
            {
                foreach(IRoomObject roomObject in roomObjects)
                {
                    if (!CanTrigger(roomObject)) return false;
                }
            }

            return true;
        }

        public bool ProcessActionsAtTile(IRoomTile roomTile)
        {
            IList<IRoomObject> roomObjects = GetActionsAtTile(roomTile);

            if (roomObjects.Count > 0)
            {
                foreach (IRoomObject roomObject in roomObjects)
                {
                    if (!CanTrigger(roomObject)) return false;
                }
            }

            return true;
        }

        public bool CanTrigger(IRoomObject roomObject)
        {
            IFurnitureWiredLogic wiredLogic = (IFurnitureWiredLogic)roomObject.Logic;

            if (!wiredLogic.CanTrigger()) return false;

            wiredLogic.OnTriggered();

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
