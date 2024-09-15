using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Storage;
using Turbo.Core.Database.Entities.Furniture;

namespace Turbo.Furniture;

public class RoomWallFurniture(
    ILogger<IRoomWallFurniture> _logger,
    IRoomFurnitureManager _roomFurnitureManager,
    IFurnitureManager _furnitureManager,
    FurnitureEntity _furnitureEntity,
    IFurnitureDefinition _furnitureDefinition,
    IStorageQueue _storageQueue)
    : RoomFurniture(_logger, _roomFurnitureManager, _furnitureManager, _furnitureEntity, _furnitureDefinition),
        IRoomWallFurniture
{
    public IRoomObjectWall RoomObject { get; private set; }

    public bool SetRoomObject(IRoomObjectWall roomObject)
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

    public string SavedWallLocation => FurnitureEntity.WallPosition;

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
                FurnitureEntity.StuffData =
                    JsonSerializer.Serialize(RoomObject.Logic.StuffData, RoomObject.Logic.StuffData.GetType());
        }

        _storageQueue.Add(FurnitureEntity);
    }
}