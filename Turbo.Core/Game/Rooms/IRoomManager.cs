using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms.Constants;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Utilities;

namespace Turbo.Core.Game.Rooms;

public interface IRoomManager : IComponent, ICyclable
{
    public Task<IRoom> CreateRoom(IPlayer player, string name, string description, string modelName, int userMax, int catId, RoomTradeType tradeType);
    public Task<IRoom> GetRoom(int id);
    public IRoom GetOnlineRoom(int id);
    public Task<IRoom> GetOfflineRoom(int id);
    public Task RemoveRoom(int id);
    public Task<IRoomModel> GetModel(int id);
    public IRoomModel GetModelByName(string name);
    public Task<List<IRoom>> GetRoomsByOwnerAsync(int ownerId, string? searchParam, string filterMode);
    public Task<List<IRoom>> GetFavoriteRoomsAsync(int playerId);
    public Task<List<IRoom>> SearchRooms(string searchTerm);
    public Task<List<IRoom>> GetRoomsByCategoriesAsync(IEnumerable<int> categoryIds);
    public Task<List<IRoom>> GetRoomsOrderedByPopularityAsync();
    public Task<List<IRoom>> GetRoomsHistoryAsync(int playerId, string? searchParam, string filterMode);
    public Task<bool> RoomExistsAsync(int roomId);
}