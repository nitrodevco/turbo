using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Database.Context;
using Turbo.Database.Entities.Messenger;

namespace Turbo.Database.Repositories.Messenger;
public class MessengerRequestsRepository(IEmulatorContext _context) : IMessengerRequestsRepository
{
    public async Task<MessengerRequestEntity> FindAsync(int id)
    {
        return await _context.MessengerRequests
            .FindAsync(id);
    }

    public async Task<List<MessengerRequestEntity>> FindAllFriendRequests(int playerId)
    {
        return await _context.MessengerRequests
            .Where(entity => entity.RequestedPlayerEntityId == playerId)
            .Include(entity => entity.PlayerEntity)
            .Include(entity => entity.RequestedPlayerEntity) // fixed: include navigation not scalar FK
            .ToListAsync();
    }

    public async Task<MessengerRequestEntity> CreateFriendRequest(int playerId, int targetPlayerId)
    {
        var existingRequest = await _context.MessengerRequests
            .FirstOrDefaultAsync(entity => entity.PlayerEntityId == playerId && entity.RequestedPlayerEntityId == targetPlayerId);

        if(existingRequest is null)
        {
            var newRequest = new MessengerRequestEntity
            {
                PlayerEntityId = playerId,
                RequestedPlayerEntityId = targetPlayerId
            };

            _context.MessengerRequests.Add(newRequest);

            await _context.SaveChangesAsync();
        }

        var request = await _context.MessengerRequests
            .Include(entity => entity.PlayerEntity)
            .Include(entity => entity.RequestedPlayerEntity)
            .FirstOrDefaultAsync(entity => entity.PlayerEntityId == playerId && entity.RequestedPlayerEntityId == targetPlayerId);

        return request;

    }

    public async Task DeleteFriendRequest(int playerId, int targetPlayerId)
    {
        var request = _context.MessengerRequests
            .FirstOrDefault(entity => entity.PlayerEntityId == playerId && entity.RequestedPlayerEntityId == targetPlayerId);

        if(request is not null)
        {
            _context.MessengerRequests.Remove(request);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteFriendRequests(IEnumerable<int> playerIds, int targetPlayerId)
    {
        var requests = _context.MessengerRequests
            .Where(entity => playerIds.Contains(entity.PlayerEntityId) && entity.RequestedPlayerEntityId == targetPlayerId);

        if(requests.Any())
        {
            _context.MessengerRequests.RemoveRange(requests);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ClearFriendRequests(int playerId)
    {
        await _context.MessengerRequests
            .Where(entity => entity.RequestedPlayerEntityId == playerId)
            .ExecuteDeleteAsync();
    }
}
