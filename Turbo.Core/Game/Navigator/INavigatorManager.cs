using System.Collections.Concurrent;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Constants;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Utilities;

namespace Turbo.Core.Game.Navigator;

public interface INavigatorManager : IComponent
{
    public Task CreateFlat(IPlayer player, string name, string description, string modelName, int maxUsers, int categoryId, RoomTradeType tradeType);
    public int GetPendingRoomId(int userId);
    public void SetPendingRoomId(int userId, int roomId, bool approved = false);
    public void ClearPendingRoomId(int userId);
    public void ClearRoomStatus(IPlayer player);
    public Task GetGuestRoomMessage(IPlayer player, int roomId, bool enterRoom = false, bool roomForward = false);

    public Task EnterRoom(IPlayer player, int roomId, string password = null, bool skipState = false, IPoint location = null);
    public Task PrepareRoomConnection(IPlayer player, IRoom room);
    public Task ContinueEnteringRoom(IPlayer player);
    public Task SendNavigatorCategories(IPlayer player);
    public Task SendNavigatorSettings(IPlayer player);
    public Task SendNavigatorMetaData(IPlayer player);
    public Task SendNavigatorLiftedRooms(IPlayer player);
    public Task SendNavigatorSavedSearches(IPlayer player);
    public Task SendNavigatorEventCategories(IPlayer player);
    public Task SendNavigatorCollapsedCategories(IPlayer player);
    public Task HandleNavigatorSearch(IPlayer player, string searchCode, string searchTerm, string filterMode);
    public Task SendOfficialRooms(IPlayer player);
    public Task SendHotelView(IPlayer player);
    public Task SendMyWorldView(IPlayer player, string? searchParam, string? filterMode);
    public Task SendCategoryRooms(IPlayer player, string searchParam);
    public Task HandleFavouriteRoomChangeAsync(int playerId, int roomId, bool isAdding);
    public Task<ConcurrentDictionary<int, byte>> GetFavoriteRoomsAsync(int playerId);
    public Task LoadFavoriteRoomsCacheAsync(int playerId);
}