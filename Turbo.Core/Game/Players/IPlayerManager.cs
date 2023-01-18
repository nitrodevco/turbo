using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Storage;
using Turbo.Core.Game.Inventory;

namespace Turbo.Core.Game.Players
{
    public interface IPlayerManager : IAsyncDisposable
    {
        public IPlayer GetPlayerById(int id);
        public IPlayer GetPlayerByUsername(string username);
        public Task<IPlayer> GetOfflinePlayerById(int id);
        public Task<IPlayer> GetOfflinePlayerByUsername(string username);
        public Task<IPlayer> CreatePlayer(int id, ISession session);
        public ValueTask RemovePlayer(int id);
        public ValueTask RemoveAllPlayers();
        public void ClearPlayerRoomStatus(IPlayer player);
        public Task EnterRoom(IPlayer player, int roomId, string password = null, bool skipState = false, IPoint location = null);
        public Task<IList<IPlayerBadge>> GetPlayerActiveBadges(int playerId);

        public IStorageQueue StorageQueue { get; }
    }
}
