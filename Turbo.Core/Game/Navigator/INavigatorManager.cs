using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Utilities;

namespace Turbo.Core.Game.Navigator;

public interface INavigatorManager : IComponent
{
    public int GetPendingRoomId(int userId);
    public void SetPendingRoomId(int userId, int roomId, bool approved = false);
    public void ClearPendingRoomId(int userId);
    public void ClearRoomStatus(IPlayer player);
    public Task GetGuestRoomMessage(IPlayer player, int roomId, bool enterRoom = false, bool roomForward = false);

    public Task EnterRoom(IPlayer player, int roomId, string password = null, bool skipState = false,
        IPoint location = null);

    public Task ContinueEnteringRoom(IPlayer player);
    public Task SendNavigatorCategories(IPlayer player);
    public Task SendNavigatorSettings(IPlayer player);
    public Task SendNavigatorMetaData(IPlayer player);
    public Task SendNavigatorLiftedRooms(IPlayer player);
    public Task SendNavigatorSavedSearches(IPlayer player);
    public Task SendNavigatorEventCategories(IPlayer player);
}