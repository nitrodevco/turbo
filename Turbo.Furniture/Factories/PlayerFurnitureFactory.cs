using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Turbo.Core.Database.Entities.Furniture;
using Turbo.Core.Database.Factories.Players;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Inventory;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Database.Context;

namespace Turbo.Furniture.Factories;

public class PlayerFurnitureFactory(
    IFurnitureManager _furnitureManager,
    IRoomObjectLogicFactory _roomObjectLogicFactory,
    IServiceProvider _provider) : IPlayerFurnitureFactory
{
    public IPlayerFurniture Create(IPlayerFurnitureContainer playerFurnitureContainer, FurnitureEntity furnitureEntity)
    {
        var furnitureDefinition = _furnitureManager.GetFurnitureDefinition(furnitureEntity.FurnitureDefinitionEntityId);

        if (furnitureDefinition == null) return null;

        var stuffDataKey = _roomObjectLogicFactory.GetStuffDataKeyForFurnitureType(furnitureDefinition.Logic);

        return ActivatorUtilities.CreateInstance<PlayerFurniture>(_provider, playerFurnitureContainer, furnitureEntity,
            furnitureDefinition, stuffDataKey);
    }

    public IPlayerFurniture CreateFromRoomFurniture(IPlayerFurnitureContainer playerFurnitureContainer,
        IRoomFurniture roomFurniture, int playerId)
    {
        if (roomFurniture is not RoomFurniture furniture) return null;

        furniture.FurnitureEntity.PlayerEntityId = playerId;
        furniture.FurnitureEntity.RoomEntityId = null;

        furniture.Save();

        return Create(playerFurnitureContainer, furniture.FurnitureEntity);
    }

    public async Task<IPlayerFurniture> CreateFromDefinitionId(IPlayerFurnitureContainer playerFurnitureContainer,
        int furnitureDefinitonId, int playerId)
    {
        // TODO here we need to pass extra data, like paint color etc
        var furnitureEntity = new FurnitureEntity
        {
            PlayerEntityId = playerId,
            FurnitureDefinitionEntityId = furnitureDefinitonId
        };

        using var scope = _provider.CreateScope();

        var context = scope.ServiceProvider.GetService<IEmulatorContext>();

        context.Add(furnitureEntity);

        await context.SaveChangesAsync();

        return Create(playerFurnitureContainer, furnitureEntity);
    }
}