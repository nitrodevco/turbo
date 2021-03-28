using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Database.Entities.Furniture;

namespace Turbo.Furniture.Factories
{
    public class FurnitureFactory : IFurnitureFactory
    {
        private readonly IServiceProvider _provider;

        public FurnitureFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IFurniture Create(IFurnitureContainer furnitureContainer, FurnitureEntity furnitureEntity)
        {
            IFurnitureManager furnitureManager = _provider.GetService<IFurnitureManager>();

            IFurnitureDefinition furnitureDefinition = furnitureManager.GetFurnitureDefinition(furnitureEntity.FurnitureDefinitionEntityId);

            if (furnitureDefinition == null) return null;

            ILogger<IFurniture> logger = _provider.GetService<ILogger<Furniture>>();
            IServiceScopeFactory scopeFactory = _provider.GetService<IServiceScopeFactory>();

            return new Furniture(logger, furnitureContainer, furnitureManager, furnitureEntity, furnitureDefinition);
        }
    }
}
