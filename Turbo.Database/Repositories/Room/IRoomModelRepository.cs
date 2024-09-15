using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Database.Entities.Room;

namespace Turbo.Database.Repositories.Room;

public interface IRoomModelRepository : IBaseRepository<RoomModelEntity>
{
    Task<RoomModelEntity> FindByNameAsync(string name);
    Task<List<RoomModelEntity>> FindAllAsync();
}