using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Database.Entities.Furniture;

namespace Turbo.Database.Repositories.Furniture
{
    public interface IFurnitureRepository : IBaseRepository<FurnitureEntity>
    {
        public Task<List<FurnitureEntity>> FindAllByRoomIdAsync(int roomId);
    }
}
