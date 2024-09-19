using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Database.Entities.Room;

namespace Turbo.Database.Repositories.Room;

public interface IRoomRepository : IBaseRepository<RoomEntity>
{
    public Task<List<RoomEntity>> GetRoomsOrderedByPopularityAsync();
}