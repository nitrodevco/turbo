using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Core.Game.Inventory;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Database.Context;
using Turbo.Database.Entities.Furniture;

namespace Turbo.Furniture.Factories
{
    public class PlayerFurnitureFactory : IPlayerFurnitureFactory
    {
        private readonly IServiceProvider _provider;

        public PlayerFurnitureFactory(
            IServiceProvider provider)
        {
            _provider = provider;
        }

        public IPlayerFurniture Create(IPlayerFurnitureContainer playerFurnitureContainer, FurnitureEntity furnitureEntity)
        {
            IFurnitureManager furnitureManager = _provider.GetService<IFurnitureManager>();

            IFurnitureDefinition furnitureDefinition = furnitureManager.GetFurnitureDefinition(furnitureEntity.FurnitureDefinitionEntityId);

            if (furnitureDefinition == null) return null;

            ILogger<IPlayerFurniture> logger = _provider.GetService<ILogger<PlayerFurniture>>();
            IServiceScopeFactory scopeFactory = _provider.GetService<IServiceScopeFactory>();
            IRoomObjectLogicFactory logicFactory = _provider.GetService<IRoomObjectLogicFactory>();

            var stuffDataKey = logicFactory.GetStuffDataKeyForFurnitureType(furnitureDefinition.Logic);

            return new PlayerFurniture(logger, playerFurnitureContainer, furnitureEntity, furnitureDefinition, stuffDataKey);
        }

        public IPlayerFurniture CreateFromRoomFurniture(IPlayerFurnitureContainer playerFurnitureContainer, IRoomFurniture roomFurniture, int playerId)
        {
            if (roomFurniture is not RoomFurniture furniture) return null;

            furniture.FurnitureEntity.PlayerEntityId = playerId;
            furniture.FurnitureEntity.RoomEntityId = null;

            furniture.Save();

            return Create(playerFurnitureContainer, furniture.FurnitureEntity);
        }

        public async Task<IPlayerFurniture> CreateFromDefinitionId(IPlayerFurnitureContainer playerFurnitureContainer, int furnitureDefinitonId, int playerId)
        {
            var furnitureEntity = new FurnitureEntity();

            furnitureEntity.PlayerEntityId = playerId;
            furnitureEntity.FurnitureDefinitionEntityId = furnitureDefinitonId;

            var emulatorContext = _provider.GetService<IEmulatorContext>();

            emulatorContext.Add(furnitureEntity);

            await emulatorContext.SaveChangesAsync();

            return Create(playerFurnitureContainer, furnitureEntity);
        }
    }
}