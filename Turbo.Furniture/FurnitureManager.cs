using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Turbo.Core.Database.Dtos;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Core.Utilities;
using Turbo.Database.Entities.Furniture;
using Turbo.Database.Repositories.Furniture;
using Turbo.Furniture.Definition;

namespace Turbo.Furniture
{
    public class FurnitureManager : Component, IFurnitureManager
    {
        private readonly ILogger<FurnitureManager> _logger;
        private readonly IFurnitureDefinitionRepository _furnitureDefinitionRepository;
        private readonly IFurnitureRepository _furnitureRepository;

        private IDictionary<int, IFurnitureDefinition> _furnitureDefinitions = new Dictionary<int, IFurnitureDefinition>();

        public FurnitureManager(
            ILogger<FurnitureManager> logger,
            IFurnitureDefinitionRepository furnitureDefinitionRepository,
            IFurnitureRepository furnitureRepository)
        {
            _logger = logger;
            _furnitureRepository = furnitureRepository;
            _furnitureDefinitionRepository = furnitureDefinitionRepository;
        }

        protected override async Task OnInit()
        {
            await LoadDefinitions();
        }

        protected override async Task OnDispose()
        {
        }

        public IFurnitureDefinition GetFurnitureDefinition(int id)
        {
            if (_furnitureDefinitions.TryGetValue(id, out IFurnitureDefinition furnitureDefinition))
            {
                return furnitureDefinition;
            }

            return null;
        }

        public async Task<TeleportPairingDto> GetTeleportPairing(int furnitureId)
        {
            return await _furnitureRepository.GetTeleportPairingAsync(furnitureId);
        }

        private async Task LoadDefinitions()
        {
            // when definitions are reloaded
            // we need all furniture to reload their definitions

            _furnitureDefinitions.Clear();

            var entities = await _furnitureDefinitionRepository.FindAllAsync();

            entities.ForEach(entity =>
            {
                IFurnitureDefinition definition = new FurnitureDefinition(entity);

                _furnitureDefinitions.Add(definition.Id, definition);
            });

            _logger.LogInformation("Loaded {0} furniture definitions", _furnitureDefinitions.Count);
        }
    }
}
