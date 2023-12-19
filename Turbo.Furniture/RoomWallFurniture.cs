using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;
using Turbo.Core.Database.Dtos;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Game.Rooms.Object.Logic.Wired;
using Turbo.Database.Entities.Furniture;
using Turbo.Core.Game.Players;

namespace Turbo.Furniture
{
    public class RoomWallFurniture : RoomFurniture, IRoomWallFurniture
    {
        public IRoomObjectWall RoomObject { get; private set; }

        public RoomWallFurniture(
            ILogger<IRoomWallFurniture> logger,
            IRoomFurnitureManager roomFurnitureManager,
            IFurnitureManager furnitureManager,
            FurnitureEntity furnitureEntity,
            IFurnitureDefinition furnitureDefinition) : base(logger, roomFurnitureManager, furnitureManager, furnitureEntity, furnitureDefinition)
        {
        }

        protected override void OnDispose()
        {
            ClearRoomObject();
        }

        protected override void OnSave()
        {
            if (RoomObject != null)
            {
                FurnitureEntity.WallPosition = RoomObject.WallLocation;
                
                if (RoomObject.Logic.StuffData != null)
                {
                    FurnitureEntity.StuffData = JsonSerializer.Serialize(RoomObject.Logic.StuffData, RoomObject.Logic.StuffData.GetType());
                }
            }
        }

        public bool SetRoomObject(IRoomObjectWall roomObject)
        {
            ClearRoomObject();

            if ((roomObject == null) || !roomObject.SetHolder(this)) return false;

            RoomObject = roomObject;

            return true;
        }

        public async Task<bool> SetupRoomObject()
        {
            if (RoomObject == null) return false;

            if (!await RoomObject.Logic.Setup(FurnitureDefinition, FurnitureEntity.StuffData)) return false;

            return true;
        }

        public void ClearRoomObject()
        {
            if(RoomObject == null) return;
            
            Save();

            RoomObject.Dispose();

            RoomObject = null;
        }

        public string SavedWallLocation => FurnitureEntity.WallPosition;
    }
}
