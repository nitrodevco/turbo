using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Database.Entities.Room;

namespace Turbo.Database.Repositories.Room
{
    public interface IRoomChatLogRepository
    {
        Task<RoomChatLogEntity> FindAsync(int id);
        Task<List<RoomChatLogEntity>> FindAllByRoomIdAsync(int roomId);
        Task<bool> AddChatLogAsync(int roomId, int playerId, string message, DateTime createdAt, DateTime updatedAt);
        Task<bool> RemoveChatLogAsync(RoomChatLogEntity entity);
    }
}