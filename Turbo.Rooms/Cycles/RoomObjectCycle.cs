using System.Threading.Tasks;
using Turbo.Core.Game.Rooms;

namespace Turbo.Rooms.Cycles;

public class RoomObjectCycle(IRoom _room) : RoomCycle(_room)
{
    public override async Task Cycle()
    {
        if (_room.RoomFurnitureManager != null)
        {
            var floorObjects = _room.RoomFurnitureManager.FloorObjects.RoomObjects;

            if (floorObjects.Count > 0)
                foreach (var floorObject in floorObjects.Values)
                    await floorObject.Logic.Cycle();

            var wallObjects = _room.RoomFurnitureManager.WallObjects.RoomObjects;

            if (wallObjects.Count > 0)
                foreach (var wallObject in wallObjects.Values)
                    await wallObject.Logic.Cycle();
        }

        if (_room.RoomUserManager != null)
        {
            var avatarObjects = _room.RoomUserManager.AvatarObjects.RoomObjects;

            if (avatarObjects.Count > 0)
                foreach (var avatarObject in avatarObjects.Values)
                    await avatarObject.Logic.Cycle();
        }
    }
}