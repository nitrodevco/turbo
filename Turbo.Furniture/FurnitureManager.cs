using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Database.Entities.Furniture;
using Turbo.Database.Repositories.Furniture;
using Turbo.Furniture.Definition;

namespace Turbo.Furniture
{
    public class FurnitureManager : IFurnitureManager
    {
        private readonly IFurnitureDefinitionRepository _furnitureDefinitionRepository;

        private Dictionary<int, IFurnitureDefinition> _furnitureDefinitions;

        public FurnitureManager(
            IFurnitureDefinitionRepository furnitureDefinitionRepository
        )
        {
            _furnitureDefinitionRepository = furnitureDefinitionRepository;

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

            List<FurnitureDefinitionEntity> entities = await _furnitureDefinitionRepository.FindAllAsync();

            if (entities.Count > 0)
            {
                foreach (FurnitureDefinitionEntity entity in entities)
                {
                    IFurnitureDefinition definition = new FurnitureDefinition(entity);

                    _furnitureDefinitions.Add(definition.Id, definition);
                }
            }

        }
    }
}
