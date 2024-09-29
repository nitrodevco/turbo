using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Utilities;

namespace Turbo.Core.Game.Rooms;

public interface IRoomManager : IComponent, ICyclable
{
    public Task<IRoom> GetRoom(int id);
    public IRoom GetOnlineRoom(int id);
    public Task<IRoom> GetOfflineRoom(int id);
    public Task RemoveRoom(int id);
    public Task<IRoomModel> GetModel(int id);
    public IRoomModel GetModelByName(string name);
    public Task<List<IRoom>> GetRoomsByOwnerAsync(int ownerId);
    public Task<List<IRoom>> GetFavoriteRoomsAsync(int playerId);
    public Task<List<IRoom>> SearchRooms(string searchTerm);
    public Task<List<IRoom>> GetRoomsByCategoriesAsync(IEnumerable<int> categoryIds);
    public Task<List<IRoom>> GetRoomsOrderedByPopularityAsync();
    public Task<bool> RoomExistsAsync(int roomId);
}