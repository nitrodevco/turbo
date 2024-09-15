using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Database.Entities.Room;

namespace Turbo.Database.Repositories.Room;

public interface IRoomRightRepository : IBaseRepository<RoomRightEntity>
{
    public Task<List<RoomRightEntity>> FindAllByRoomIdAsync(int roomId);
    public Task<bool> GiveRightsToPlayerIdAsync(int roomId, int playerId);
    public Task<bool> RemoveRightsForPlayerIdAsync(int roomId, int playerId);
}