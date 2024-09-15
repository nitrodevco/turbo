using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Database.Entities.Furniture;

namespace Turbo.Database.Repositories.Furniture;

public interface IFurnitureDefinitionRepository : IBaseRepository<FurnitureDefinitionEntity>
{
    public Task<List<FurnitureDefinitionEntity>> FindAllAsync();
}