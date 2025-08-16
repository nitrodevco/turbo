using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Database.Entities.Messenger;

namespace Turbo.Database.Repositories.Messenger;

public interface IMessengerFriendsRepository
{
    Task<List<MessengerFriendEntity>> FindAllFriends(int playerId);
    Task<(MessengerFriendEntity FriendBuddy, MessengerFriendEntity MeAsBuddy)> CreateFriendship(int playerId, int friendId);
    Task DeleteFriendship(int playerId, int friendId);
    Task DeleteFriendships(int playerId, IEnumerable<int> friendIds);
}
