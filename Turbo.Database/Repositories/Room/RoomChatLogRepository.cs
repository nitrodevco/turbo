using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Database.Context;
using Turbo.Database.Entities.Room;

namespace Turbo.Database.Repositories.Room
{
    public class RoomChatLogRepository(IEmulatorContext context) : IRoomChatLogRepository
    {
        private readonly IEmulatorContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<RoomChatLogEntity> FindAsync(int id)
        {
            return await _context.RoomChatLogs
                .FirstOrDefaultAsync(entity => entity.Id == id);
        }

        public async Task<List<RoomChatLogEntity>> FindAllByRoomIdAsync(int roomId)
        {
            return await _context.RoomChatLogs
                .Where(entity => entity.RoomEntityId == roomId)
                .ToListAsync();
        }

        public async Task<bool> AddChatLogAsync(int roomId, int playerId, string message, DateTime createdAt, DateTime updatedAt)
        {
            var entity = new RoomChatLogEntity
            {
                RoomEntityId = roomId,
                PlayerEntityId = playerId,
                Message = message,
                DateCreated = createdAt,
                DateUpdated = updatedAt
            };

            _context.RoomChatLogs.Add(entity);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveChatLogAsync(RoomChatLogEntity entity)
        {
            if (entity == null) return false;

            _context.RoomChatLogs.Remove(entity);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}