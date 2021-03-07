using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Database.Entities.Furniture;

namespace Turbo.Database.Repositories.Furniture
{
    public interface IFurnitureDefinitionRepository : IBaseRepository<FurnitureDefinitionEntity>
    {
        Task<List<FurnitureDefinitionEntity>> FindAllAsync();
    }
}
