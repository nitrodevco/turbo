using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Core.Game.Messenger.Constants;
using Turbo.Database.Context;
using Turbo.Database.Entities.Messenger;

namespace Turbo.Database.Repositories.Messenger;

public class MessengerFriendsRepository(
    IEmulatorContext context
) : IMessengerFriendsRepository
{
    public async Task<List<MessengerFriendEntity>> FindAllFriends(int playerId)
    {
        return await context.MessengerFriends
            .Where(entity => entity.PlayerEntityId == playerId)
            .Include(entity => entity.PlayerEntity)
            .Include(entity => entity.FriendPlayerEntity)
            .ToListAsync();
    }

    public async Task<(MessengerFriendEntity FriendBuddy, MessengerFriendEntity MeAsBuddy)> CreateFriendship(int playerId, int friendId)
    {
        if (playerId == friendId)
        {
            return (null, null);
        }

        var existing = await context.MessengerFriends
            .Where(friendship =>
                 (friendship.PlayerEntityId == playerId && friendship.FriendPlayerEntityId == friendId) ||
                (friendship.PlayerEntityId == friendId && friendship.FriendPlayerEntityId == playerId)
            )
            .ToListAsync();

        var friendBuddy = existing.FirstOrDefault(friendship => friendship.PlayerEntityId == playerId);
        var meAsBuddy = existing.FirstOrDefault(friendship => friendship.PlayerEntityId == friendId);

        var toAdd = new List<MessengerFriendEntity>(capacity: 2);

        if (friendBuddy is null)
        {
            friendBuddy = new MessengerFriendEntity
            {
                PlayerEntityId = playerId,
                FriendPlayerEntityId = friendId,
                RelationType = MessengerFriendRelationEnum.Zero
            };

            toAdd.Add(friendBuddy);
        }

        if (meAsBuddy is null)
        {
            meAsBuddy = new MessengerFriendEntity
            {
                PlayerEntityId = friendId,
                FriendPlayerEntityId = playerId,
                RelationType = MessengerFriendRelationEnum.Zero
            };
            toAdd.Add(meAsBuddy);
        }

        if (toAdd.Count > 0)
        {
            await context.MessengerFriends.AddRangeAsync(toAdd);
            await context.SaveChangesAsync();
        }

        var reloaded = await context.MessengerFriends
            .Where(friendship =>
                 (friendship.PlayerEntityId == playerId && friendship.FriendPlayerEntityId == friendId) ||
                (friendship.PlayerEntityId == friendId && friendship.FriendPlayerEntityId == playerId)
            )
            .Include(entity => entity.PlayerEntity)
            .Include(entity => entity.FriendPlayerEntity)
            .AsNoTracking()
            .ToListAsync();

        var friendBuddyReloaded = reloaded.FirstOrDefault(friendship => friendship.PlayerEntityId == playerId);
        var meAsBuddyReloaded = reloaded.FirstOrDefault(friendship => friendship.PlayerEntityId == friendId);

        if (friendBuddyReloaded is null || meAsBuddyReloaded is null)
        {
            return (null, null);
        }

        return (friendBuddyReloaded, meAsBuddyReloaded);
    }

    public async Task DeleteFriendship(int playerId, int friendId)
    {
        var friendship = await context.MessengerFriends
            .Where(entity =>
                (entity.PlayerEntityId == playerId && entity.FriendPlayerEntityId == friendId) ||
                (entity.PlayerEntityId == friendId && entity.FriendPlayerEntityId == playerId))
            .ToListAsync();

        context.MessengerFriends.RemoveRange(friendship);

        await context.SaveChangesAsync();
    }

    public async Task DeleteFriendships(int playerId, IEnumerable<int> friendIds)
    {
        var friendships = await context.MessengerFriends
            .Where(entity =>
                (entity.PlayerEntityId == playerId && friendIds.Contains(entity.FriendPlayerEntityId)) ||
                (friendIds.Contains(entity.PlayerEntityId) && entity.FriendPlayerEntityId == playerId))
            .ToListAsync();
        context.MessengerFriends.RemoveRange(friendships);
        await context.SaveChangesAsync();
    }
}
