using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Database.Entities.Logs.Chat;

namespace Turbo.Database.Repositories.Logs.Chat;

public interface IConsoleChatLogsRepository
{
    Task<List<ConsoleChatLogEntity>> GetConsoleChatHistory(int playerId, int friendId, int offsetId = -1, int limit = 3);
    Task<List<ConsoleChatLogEntity>> GetPendingConsoleChatLogs(int playerId);
    Task<ConsoleChatLogEntity> CreateConsoleChatLog(int senderId, int recipientId, int confirmationId, string message, DateTime sentAt);
    Task<ConsoleChatLogEntity> SetDelivered(int consoleChatId, DateTime deliveredAt);
    Task DeleteConsoleChatHistory(int playerId, int friendId);
    Task MarkConversationDelivered(int recipientId, int senderId, DateTime deliveredAt);
}
