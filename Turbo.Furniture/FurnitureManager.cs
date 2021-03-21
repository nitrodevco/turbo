using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Database.Entities.Furniture;
using Turbo.Database.Repositories.Furniture;
using Turbo.Furniture.Definition;

namespace Turbo.Furniture
{
    public class FurnitureManager : IFurnitureManager
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<FurnitureManager> _logger;

        private IDictionary<int, IFurnitureDefinition> _furnitureDefinitions;

        public FurnitureManager(
            ILogger<FurnitureManager> logger,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = scopeFactory;

            _furnitureDefinitions = new Dictionary<int, IFurnitureDefinition>();
        }

        public async ValueTask InitAsync()
        {
            await LoadDefinitions();
        }

        public async ValueTask DisposeAsync()
        {

        }

        private async ValueTask LoadDefinitions()
        {
            // when definitions are reloaded
            // we need all furniture to reload their definitions

            _furnitureDefinitions.Clear();
            
            List<FurnitureDefinitionEntity> entities;
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var furnitureDefinitionRepository = scope.ServiceProvider.GetService<IFurnitureDefinitionRepository>();
                entities = await furnitureDefinitionRepository.FindAllAsync();
            }

            entities.ForEach(entity =>
            {
                IFurnitureDefinition definition = new FurnitureDefinition(entity);

                _furnitureDefinitions.Add(definition.Id, definition);
            });
        }
    }
}
