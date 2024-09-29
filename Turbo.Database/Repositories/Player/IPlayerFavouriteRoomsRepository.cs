using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Database.Entities.Players;

namespace Turbo.Database.Repositories.Player;

public interface IPlayerFavouriteRoomsRepository : IBaseRepository<PlayerFavouriteRoomsEntity>
{
    // Add a favorite room for a player
    Task<bool> AddFavoriteRoomAsync(int playerId, int roomId);

    // Remove a favorite room for a player
    Task<bool> RemoveFavoriteRoomAsync(int playerId, int roomId);

    // Check if a room is a favorite for a player
    Task<bool> IsFavoriteRoomAsync(int playerId, int roomId);

    // Get all favorite rooms for a player
    Task<List<int>> GetFavoriteRoomsAsync(int playerId);
}