using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Database.Entities.Messenger;

namespace Turbo.Database.Repositories.Messenger;

public interface IMessengerRequestsRepository
{
    Task<List<MessengerRequestEntity>> FindAllFriendRequests(int playerId);
    Task<MessengerRequestEntity> CreateFriendRequest(int playerId, int targetPlayerId);
    Task DeleteFriendRequest(int playerId, int targetPlayerId);
    Task DeleteFriendRequests(IEnumerable<int> playerIds, int targetPlayerId);
    Task ClearFriendRequests(int playerId);
}
