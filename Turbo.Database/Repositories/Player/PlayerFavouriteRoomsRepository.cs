using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Core.Database.Entities.Players;
using Turbo.Database.Context;

namespace Turbo.Database.Repositories.Player;

public class PlayerFavouriteRoomsRepository(IEmulatorContext _context) : IPlayerFavouriteRoomsRepository
{
    public async Task<bool> AddFavoriteRoomAsync(int playerId, int roomId)
    {
        if (await IsFavoriteRoomAsync(playerId, roomId)) return false;

        var favoriteRoom = new PlayerFavouriteRoomsEntity()
        {
            PlayerId = playerId,
            RoomId = roomId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.PlayerFavouriteRooms.Add(favoriteRoom);
        await _context.SaveChangesAsync();

        return true;
    }

    // Remove a favorite room for a player
    public async Task<bool> RemoveFavoriteRoomAsync(int playerId, int roomId)
    {
        var favoriteRoom = await _context.PlayerFavouriteRooms
            .FirstOrDefaultAsync(f => f.PlayerId == playerId && f.RoomId == roomId);

        if (favoriteRoom == null) return false;

        _context.PlayerFavouriteRooms.Remove(favoriteRoom);
        await _context.SaveChangesAsync();

        return true;
    }

    // Check if a room is a favorite for a player
    public async Task<bool> IsFavoriteRoomAsync(int playerId, int roomId) => await _context.PlayerFavouriteRooms
        .AnyAsync(f => f.PlayerId == playerId && f.RoomId == roomId);

    // Get all favorite rooms for a player
    public async Task<List<int>> GetFavoriteRoomsAsync(int playerId) => await _context.PlayerFavouriteRooms
        .AsNoTracking()
        .Where(f => f.PlayerId == playerId)
        .Select(f => f.RoomId)
        .ToListAsync();

    public Task<PlayerFavouriteRoomsEntity> FindAsync(int id) => throw new NotImplementedException();
}