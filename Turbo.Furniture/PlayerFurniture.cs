using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Furniture.Data;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Core.Game.Inventory;
using Turbo.Core.Database.Entities.Furniture;
using Turbo.Furniture.Data;

namespace Turbo.Furniture;

public class PlayerFurniture(
    ILogger<IPlayerFurniture> _logger,
    IPlayerFurnitureContainer _playerFurnitureContainer,
    FurnitureEntity _furnitureEntity,
    IFurnitureDefinition _furnitureDefinition,
    StuffDataKey _stuffDataKey) : IPlayerFurniture
{
    protected bool _isDisposing;
    public FurnitureEntity FurnitureEntity => _furnitureEntity;

    public IStuffData StuffData { get; } =
        StuffDataFactory.CreateStuffDataFromJson((int)_stuffDataKey, _furnitureEntity.StuffData);

    public void Dispose()
    {
        if (Disposed || _isDisposing) return;

        _isDisposing = true;
        _playerFurnitureContainer?.RemoveFurniture(this);
        _playerFurnitureContainer = null;
        _isDisposing = false;
    }

    public int Id => _furnitureEntity.Id;
    public IFurnitureDefinition FurnitureDefinition => _furnitureDefinition;
    public bool Disposed => _playerFurnitureContainer == null;
}