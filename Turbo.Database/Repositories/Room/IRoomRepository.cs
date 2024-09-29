using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Database.Entities.Room;
using Turbo.Core.Game.Navigator.Constants;
using Turbo.Core.Game.Rooms.Constants;

namespace Turbo.Database.Repositories.Room;

public interface IRoomRepository : IBaseRepository<RoomEntity>
{
    Task<RoomEntity> CreateRoom(int ownerId, string name, string description, int modelId, int maxUsers, int categoryId, RoomTradeType tradeSetting);
    Task<List<RoomEntity>> FindRoomsByOwnerIdAsync(int ownerId);
    Task<List<RoomEntity>> GetRoomsOrderedByPopularityAsync();
    Task<List<RoomEntity>> SearchRoomsByNameAsync(string searchTerm);
    Task<List<RoomEntity>> GetRoomsByCategoryIdsAsync(IEnumerable<int> categoryIds);
    Task<List<RoomEntity>> GetRoomsByStateAsync(RoomStateType state);
    Task<bool> RoomExistsAsync(int roomId);
}