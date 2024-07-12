using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Database.Entities.Room;

namespace Turbo.Database.Repositories.Room
{
    public interface IChatlogRepository
    {
        Task<ChatlogEntity> FindAsync(int id);
        Task<List<ChatlogEntity>> FindAllByRoomIdAsync(int roomId);
        Task<bool> AddChatlogAsync(int? roomId, int playerId, string message, string type, DateTime createdAt, DateTime updatedAt, int? recipientUserId);
        Task<bool> RemoveChatlogAsync(ChatlogEntity entity);
    }
}