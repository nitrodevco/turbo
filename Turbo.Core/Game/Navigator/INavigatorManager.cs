using System;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;

namespace Turbo.Core.Game.Navigator
{
    public interface INavigatorManager : IAsyncInitialisable, IAsyncDisposable
    {
        public int GetPendingRoomId(int userId);
        public void SetPendingRoomId(int userId, int roomId, bool approved = false);
        public void ClearPendingRoomId(int userId);
        public void ClearRoomStatus(IPlayer player);
        public Task GetGuestRoomMessage(IPlayer player, int roomId, bool enterRoom = false, bool roomForward = false);
        public Task EnterRoom(IPlayer player, int roomId, string password = null, bool skipState = false);
        public Task ContinueEnteringRoom(IPlayer player);
        public Task SendNavigatorMetaData(IPlayer player);
        public Task SendNavigatorLiftedRooms(IPlayer player);
    }
}
