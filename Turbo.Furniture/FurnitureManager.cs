using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Turbo.Core.Database.Dtos;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Core.Utilities;
using Turbo.Database.Repositories.Furniture;
using Turbo.Furniture.Definition;

namespace Turbo.Furniture;

public class FurnitureManager(
    ILogger<FurnitureManager> _logger,
    IServiceScopeFactory _serviceScopeFactory) : Component, IFurnitureManager
{
    private readonly IDictionary<int, IFurnitureDefinition> _furnitureDefinitions =
        new Dictionary<int, IFurnitureDefinition>();

    public IFurnitureDefinition GetFurnitureDefinition(int id)
    {
        if (_furnitureDefinitions.TryGetValue(id, out var furnitureDefinition)) return furnitureDefinition;

        return null;
    }

    public async Task<TeleportPairingDto> GetTeleportPairing(int furnitureId)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var furnitureRepository = scope.ServiceProvider.GetService<IFurnitureRepository>();

        return await furnitureRepository.GetTeleportPairingAsync(furnitureId);
    }

    protected override async Task OnInit()
    {
        await LoadDefinitions();
    }

    protected override async Task OnDispose()
    {
    }

    private async Task LoadDefinitions()
    {
        // TODO when definitions are reloaded we need all furniture to reload their definitions

        _furnitureDefinitions.Clear();

        using var scope = _serviceScopeFactory.CreateScope();

        var furnitureDefinitionRepository = scope.ServiceProvider.GetService<IFurnitureDefinitionRepository>();

        var entities = await furnitureDefinitionRepository.FindAllAsync();

        entities.ForEach(entity =>
        {
            var definition = new FurnitureDefinition(entity);

            _furnitureDefinitions.Add(definition.Id, definition);
        });

        _logger.LogInformation("Loaded {0} furniture definitions", _furnitureDefinitions.Count);
    }
}