using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Turbo.Core.Game.Rooms.Object.Logic
{
    public interface IFurnitureWallLogic : IFurnitureLogic
    {
        public IRoomObjectWall RoomObject { get; }
        public bool SetRoomObject(IRoomObjectWall roomObject);
    }
}