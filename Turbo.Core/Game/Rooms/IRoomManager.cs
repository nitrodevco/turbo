using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;
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
    public Task<List<IRoomDetails>> GetRoomsByCriteria(
        int? ownerId = null,
        string searchText = null,
        string tag = null,
        string roomName = null,
        string groupName = null,
        string ownerName = null,
        string category = null,
        int? searchType = null,
        IPlayer player = null,
        bool popularRooms = false,
        bool highestScore = false,
        bool friendsRooms = false,
        bool whereFriendsAre = false,
        bool favourites = false,
        bool recommended = false,
        int maxResults = 50);
}