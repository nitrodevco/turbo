using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Rooms.Utils
{
    public class RollerData : IRollerData
    {
        private readonly IRoom _room;

        public IPoint Location { get; private set; }
        public IPoint LocationNext { get; private set; }

        public IRoomObject Roller { get; set; }
        public IDictionary<int, IRollerItemData> Users { get; private set; }
        public IDictionary<int, IRollerItemData> Furniture { get; private set; }

        public RollerData(IRoom room, IPoint location, IPoint locationNext)
        {
            _room = room;

            Location = location;
            LocationNext = locationNext;

            Users = new Dictionary<int, IRollerItemData>();
            Furniture = new Dictionary<int, IRollerItemData>();
        }

        public void RemoveRoomObject(IRoomObject roomObject)
        {
            if(roomObject.Logic is IMovingAvatarLogic)
            {
                Users.Remove(roomObject.Id);
            }

            else if(roomObject.Logic is IFurnitureLogic)
            {
                Furniture.Remove(roomObject.Id);
            }
        }

        public void CommitRoll()
        {
            IRoomTile currentTile = _room.RoomMap.GetTile(Location);
            IRoomTile nextTile = _room.RoomMap.GetTile(LocationNext);

            if((currentTile == null) || (nextTile == null)) return;

            if(Users.Count > 0)
            {
                foreach(IRollerItemData rollerItemData in Users.Values)
                {
                    IRoomObject roomObject = rollerItemData.RoomObject;

                    if (roomObject.Logic is IMovingAvatarLogic avatarLogic)
                    {
                        if(!avatarLogic.IsRolling || !roomObject.Location.Compare(Location) || (avatarLogic.RollerData != this))
                        {
                            Users.Remove(roomObject.Id);

                            avatarLogic.RollerData = null;

                            continue;
                        }

                        avatarLogic.GetCurrentTile()?.RemoveRoomObject(roomObject);

                        roomObject.Location.X = LocationNext.X;
                        roomObject.Location.Y = LocationNext.Y;
                        roomObject.Location.Z = rollerItemData.HeightNext;

                        avatarLogic.GetCurrentTile()?.AddRoomObject(roomObject);
                    }
                }
            }

            if (Furniture.Count > 0)
            {
                foreach (IRollerItemData rollerItemData in Furniture.Values)
                {
                    IRoomObject roomObject = rollerItemData.RoomObject;

                    if (roomObject.Logic is IFurnitureLogic furnitureLogic)
                    {
                        if (!furnitureLogic.IsRolling || !roomObject.Location.Compare(Location) || (furnitureLogic.RollerData != this))
                        {
                            Furniture.Remove(roomObject.Id);

                            furnitureLogic.RollerData = null;

                            continue;
                        }

                        furnitureLogic.GetCurrentTile()?.RemoveRoomObject(roomObject);

                        roomObject.Location.X = LocationNext.X;
                        roomObject.Location.Y = LocationNext.Y;
                        roomObject.Location.Z = rollerItemData.HeightNext;

                        if(roomObject.RoomObjectHolder is IFurniture furniture) furniture.Save();

                        furnitureLogic.GetCurrentTile()?.AddRoomObject(roomObject);
                    }
                }
            }
        }

        public void CompleteRoll()
        {
            if (Users.Count > 0)
            {
                foreach (IRollerItemData rollerItemData in Users.Values)
                {
                    IRoomObject roomObject = rollerItemData.RoomObject;

                    if (roomObject.Logic is IRollingObjectLogic rollingLogic)
                    {
                        rollingLogic.RollerData = null;
                    }
                }
            }

            if (Furniture.Count > 0)
            {
                foreach (IRollerItemData rollerItemData in Furniture.Values)
                {
                    IRoomObject roomObject = rollerItemData.RoomObject;

                    if (roomObject.Logic is IRollingObjectLogic rollingLogic)
                    {
                        rollingLogic.RollerData = null;
                    }
                }
            }
        }
    }
}
