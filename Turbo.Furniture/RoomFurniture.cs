using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Database.Entities.Furniture;

namespace Turbo.Furniture;

public abstract class RoomFurniture(
    ILogger<IRoomFurniture> _logger,
    IRoomFurnitureManager _roomFurnitureManager,
    IFurnitureManager _furnitureManager,
    FurnitureEntity _furnitureEntity,
    IFurnitureDefinition _furnitureDefinition) : IRoomFurniture
{
    private IRoom _room;
    public FurnitureEntity FurnitureEntity { get; } = _furnitureEntity;
    public string PlayerName { get; private set; }
    private bool _isDisposing { get; set; }

    public int Id => FurnitureEntity.Id;
    public RoomObjectHolderType Type => RoomObjectHolderType.Furniture;
    public int PlayerId => FurnitureEntity.PlayerEntityId;
    public IFurnitureDefinition FurnitureDefinition { get; } = _furnitureDefinition;

    public void Save()
    {
        OnSave();
    }

    public void SetRoom(IRoom room)
    {
        if (room == null)
        {
            _room = null;

            if (FurnitureEntity.RoomEntityId != null)
            {
                FurnitureEntity.RoomEntityId = null;

                Save();
            }

            return;
        }

        _room = room;

        if (FurnitureEntity.RoomEntityId == room.Id) return;

        FurnitureEntity.RoomEntityId = room.Id;

        Save();
    }

    public bool SetPlayer(IPlayer player)
    {
        if (player == null) return false;

        return SetPlayer(player.Id, player.Name);
    }

    public bool SetPlayer(int playerId, string playerName = "")
    {
        if (playerId <= 0) return false;

        if (FurnitureEntity.PlayerEntityId != playerId)
        {
            FurnitureEntity.PlayerEntityId = playerId;

            Save();
        }

        if (playerName.Length > 0) PlayerName = playerName;

        return true;
    }

    public string LogicType => FurnitureDefinition.Logic;

    public void Dispose()
    {
        if (_isDisposing) return;

        _isDisposing = true;

        OnDispose();
    }

    protected abstract void OnDispose();

    protected abstract void OnSave();
}