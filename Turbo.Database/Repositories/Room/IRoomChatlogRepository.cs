using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Database.Entities.Room;

namespace Turbo.Database.Repositories.Room;

public interface IRoomChatlogRepository : IBaseRepository<RoomChatlogEntity>
{
    public Task<List<RoomChatlogEntity>> FindAllByRoomIdAsync(int roomId);
    public Task<bool> AddRoomChatlogAsync(int roomId, int playerId, string message, int? targetPlayerId);
}