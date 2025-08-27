using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Database.Context;
using Turbo.Database.Entities.Logs.History;

namespace Turbo.Database.Repositories.Logs.History;

public class ConsoleChatLogsRepository(
    IEmulatorContext context
) : IConsoleChatLogsRepository
{
    public async Task<ConsoleChatLogEntity> CreateConsoleChatLog(int senderId, int recipientId, int confirmationId, string message, DateTime sentAt)
    {
        if (senderId == recipientId || string.IsNullOrWhiteSpace(message))
        {
            return null;
        }

        var entity = new ConsoleChatLogEntity
        {
            SenderEntityId = senderId,
            RecipientEntityId = recipientId,
            ConfirmationId = confirmationId,
            Message = message,
            SentAt = sentAt == default ? DateTime.UtcNow : sentAt.ToUniversalTime(),
            IsDelivered = false,
            DeliveredAt = null
        };

        context.Add(entity);
        await context.SaveChangesAsync();

        return await context.ConsoleChatLogs
            .Include(log => log.SenderEntity)
            .Include(log => log.RecipientEntity)
            .FirstOrDefaultAsync(log => log.Id == entity.Id);
    }

    public async Task DeleteConsoleChatHistory(int playerId, int friendId)
    {
        var consoleChatLogs = await context.ConsoleChatLogs
            .Where(log => (log.SenderEntityId == playerId && log.RecipientEntityId == friendId) ||
                          (log.SenderEntityId == friendId && log.RecipientEntityId == playerId))
            .ToListAsync();

        if (consoleChatLogs.Count == 0) return;

        context.ConsoleChatLogs.RemoveRange(consoleChatLogs);
        await context.SaveChangesAsync();
    }

    public async Task<List<ConsoleChatLogEntity>> GetConsoleChatHistory(int playerId, int friendId, int offsetId = -1, int limit = 10)
    {
        var baseQuery = context.ConsoleChatLogs
            .Where(log =>
                (log.SenderEntityId == playerId && log.RecipientEntityId == friendId) ||
                (log.SenderEntityId == friendId && log.RecipientEntityId == playerId));

        if (offsetId != -1)
        {
            var anchor = await context.ConsoleChatLogs
                .Where(log => log.Id == offsetId &&
                                (
                                    (log.SenderEntityId == playerId && log.RecipientEntityId == friendId) ||
                                    (log.SenderEntityId == friendId && log.RecipientEntityId == playerId)
                                ))
                .Select(log => new { log.SentAt, log.Id })
                .SingleOrDefaultAsync();

            if (anchor is null) return new List<ConsoleChatLogEntity>();

            baseQuery = baseQuery.Where(log =>
                log.SentAt < anchor.SentAt ||
                (log.SentAt == anchor.SentAt && log.Id < anchor.Id));
        }

        return await baseQuery
            .OrderByDescending(log => log.SentAt)
            .ThenByDescending(log => log.Id)
            .Take(limit)
            .OrderBy(log => log.SentAt)
            .ThenBy(log => log.Id)
            .Include(log => log.SenderEntity)
            .Include(log => log.RecipientEntity)
            .ToListAsync();
    }

    public async Task<List<ConsoleChatLogEntity>> GetPendingConsoleChatLogs(int playerId)
    {
        return await context.ConsoleChatLogs
            .Where(log => log.RecipientEntityId == playerId && !log.IsDelivered)
            .Include(log => log.SenderEntity)
            .Include(log => log.RecipientEntity)
            .OrderBy(log => log.SentAt)
            .ToListAsync();
    }

    public async Task<ConsoleChatLogEntity> SetDelivered(int consoleChatId, DateTime deliveredAt)
    {
        var utc = deliveredAt == default ? DateTime.UtcNow : deliveredAt.ToUniversalTime();

        await context.ConsoleChatLogs
            .Where(l => l.Id == consoleChatId && !l.IsDelivered)
            .ExecuteUpdateAsync(s => s
                .SetProperty(l => l.IsDelivered, true)
                .SetProperty(l => l.DeliveredAt, utc));

        return await context.ConsoleChatLogs
            .Include(l => l.SenderEntity)
            .Include(l => l.RecipientEntity)
            .FirstOrDefaultAsync(l => l.Id == consoleChatId);
    }

    public async Task MarkConversationDelivered(int recipientId, int senderId, DateTime deliveredAt)
    {
        var utc = deliveredAt == default ? DateTime.UtcNow : deliveredAt.ToUniversalTime();

        await context.ConsoleChatLogs
            .Where(log => log.SenderEntityId == senderId && log.RecipientEntityId == recipientId && !log.IsDelivered)
            .ExecuteUpdateAsync(s => s
                .SetProperty(l => l.IsDelivered, true)
                .SetProperty(l => l.DeliveredAt, utc));
    }
}