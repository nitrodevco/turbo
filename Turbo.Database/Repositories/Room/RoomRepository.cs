using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Core.Database.Entities.Room;
using Turbo.Core.Game.Rooms.Constants;
using Turbo.Database.Context;

namespace Turbo.Database.Repositories.Room;

public class RoomRepository(IEmulatorContext context) : IRoomRepository
{
    public async Task<RoomEntity> CreateRoom(int ownerId, string name, string description, int modelId, int maxUsers, int categoryId, RoomTradeType tradeSetting)
    {
        var entity = new RoomEntity
        {
            Name = name,
            Description = description,
            PlayerEntityId = ownerId,
            UsersMax = maxUsers,
            NavigatorCategoryEntityId = categoryId,
            RoomModelEntityId = modelId,
            TradeType = tradeSetting
        };

        context.Add(entity);

        await context.SaveChangesAsync();

        return entity;
    }
    
    public async Task<RoomEntity> FindAsync(int id) => await context.Rooms
        .FirstOrDefaultAsync(room => room.Id == id);

    public async Task<List<RoomEntity>> FindRoomsByOwnerIdAsync(int ownerId) => await context.Rooms
        .Where(r => r.PlayerEntityId == ownerId)
        .ToListAsync();

    public async Task<List<RoomEntity>> GetRoomsOrderedByPopularityAsync() => await context.Rooms
        .OrderByDescending(room => room.UsersNow)
        .ToListAsync();

    public async Task<List<RoomEntity>> SearchRoomsByNameAsync(string searchTerm) => await context.Rooms
        .AsNoTracking()
        .Where(r => EF.Functions.Like(r.Name, $"%{searchTerm}%"))
        .ToListAsync();

    public async Task<List<RoomEntity>> GetRoomsByCategoryIdsAsync(IEnumerable<int> categoryIds)
    {
        var idsList = categoryIds.ToList();

        if (!idsList.Any())
        {
            return new List<RoomEntity>();
        }

        return await context.Rooms
            .Where(r => r.NavigatorCategoryEntityId.HasValue && idsList.Contains(r.NavigatorCategoryEntityId.Value))
            .OrderByDescending(r => r.UsersNow)
            .Take(50)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<RoomEntity>> GetRoomsByStateAsync(RoomStateType state) => await context.Rooms
        .Where(r => r.RoomState == state)
        .ToListAsync();

    public async Task<bool> RoomExistsAsync(int roomId) => await context.Rooms.AnyAsync(r => r.Id == roomId);
}