using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Database.Context;
using Turbo.Database.Entities.Room;

namespace Turbo.Database.Repositories.Room
{
    public class ChatlogRepository(IEmulatorContext context) : IChatlogRepository
    {
        private readonly IEmulatorContext _context = context ?? throw new ArgumentNullException(nameof(context));
        
        public async Task<ChatlogEntity> FindAsync(int id)
        {
            return await _context.Chatlogs
                .FirstOrDefaultAsync(entity => entity.Id == id);
        }

        public async Task<List<ChatlogEntity>> FindAllByRoomIdAsync(int roomId)
        {
            return await _context.Chatlogs
                .Where(entity => entity.RoomEntityId == roomId)
                .ToListAsync();
        }

        public async Task<bool> AddChatlogAsync(int? roomId, int playerId, string message, string type, DateTime createdAt, DateTime updatedAt, int? recipientUserId = null)
        {
            var entity = new ChatlogEntity
            {
                RoomEntityId = roomId,
                PlayerEntityId = playerId,
                RecipientUserId = recipientUserId,
                Message = message,
                Type = type,
                DateCreated = createdAt,
                DateUpdated = updatedAt,
            };

            _context.Chatlogs.Add(entity);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveChatlogAsync(ChatlogEntity entity)
        {
            if (entity == null) return false;

            _context.Chatlogs.Remove(entity);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}