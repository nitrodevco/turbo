using System;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Core.Networking.Game.Clients;

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
    }
}
