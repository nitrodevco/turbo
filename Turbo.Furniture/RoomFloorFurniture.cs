using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Turbo.Core.Database.Dtos;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Storage;
using Turbo.Core.Database.Entities.Furniture;

namespace Turbo.Furniture;

public class RoomFloorFurniture(
    ILogger<IRoomFloorFurniture> _logger,
    IRoomFurnitureManager _roomFurnitureManager,
    IFurnitureManager _furnitureManager,
    FurnitureEntity _furnitureEntity,
    IFurnitureDefinition _furnitureDefinition,
    IStorageQueue _storageQueue)
    : RoomFurniture(_logger, _roomFurnitureManager, _furnitureManager, _furnitureEntity, _furnitureDefinition),
        IRoomFloorFurniture
{
    public IRoomObjectFloor RoomObject { get; private set; }

    public bool SetRoomObject(IRoomObjectFloor roomObject)
    {
        ClearRoomObject();

        if (roomObject == null || !roomObject.SetHolder(this)) return false;

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
        if (RoomObject == null) return;

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
                FurnitureEntity.StuffData =
                    JsonSerializer.Serialize(RoomObject.Logic.StuffData, RoomObject.Logic.StuffData.GetType());
        }

        _storageQueue.Add(FurnitureEntity);
    }
}