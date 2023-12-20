using System;
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
using Turbo.Database.Entities.Furniture;
using Turbo.Core.Game.Players;

namespace Turbo.Furniture
{
    public class RoomFloorFurniture : RoomFurniture, IRoomFloorFurniture
    {
        public IRoomObjectFloor RoomObject { get; private set; }

        public RoomFloorFurniture(
            ILogger<IRoomFloorFurniture> logger,
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
                FurnitureEntity.X = RoomObject.Location.X;
                FurnitureEntity.Y = RoomObject.Location.Y;
                FurnitureEntity.Z = RoomObject.Location.Z;
                FurnitureEntity.Rotation = RoomObject.Location.Rotation;

                if (RoomObject.Logic.StuffData != null)
                {
                    FurnitureEntity.StuffData = JsonSerializer.Serialize(RoomObject.Logic.StuffData, RoomObject.Logic.StuffData.GetType());
                }
            }
        }

        public bool SetRoomObject(IRoomObjectFloor roomObject)
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

        public async Task<TeleportPairingDto> GetTeleportPairing()
        {
            return await _furnitureManager.GetTeleportPairing(Id);
        }

        public int SavedX => FurnitureEntity.X;

        public int SavedY => FurnitureEntity.Y;

        public double SavedZ => FurnitureEntity.Z;

        public Rotation SavedRotation => FurnitureEntity.Rotation;
    }
}
