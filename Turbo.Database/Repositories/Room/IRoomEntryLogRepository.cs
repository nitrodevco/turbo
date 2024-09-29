using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Database.Entities.Room;

namespace Turbo.Database.Repositories.Room;

public interface IRoomEntryLogRepository : IBaseRepository<RoomEntryLogEntity>
{
    public Task<List<RoomEntity>> GetLatestByPlayerIdAsync(int playerId, int maximumCount);
    public Task<List<RoomEntity>> GetFrequentRoomsByPlayerIdAsync(int playerId, int frequency, int maximumCount);
    public Task<bool> AddRoomEntryLogAsync(int roomId, int playerId);
}