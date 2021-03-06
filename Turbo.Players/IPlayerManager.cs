using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core;
using Turbo.Packets.Sessions;

namespace Turbo.Players
{
    public interface IPlayerManager : IAsyncDisposable
    {
        public IPlayer GetPlayerById(int id);
        public IPlayer GetPlayerByUsername(string username);
        public Task<IPlayer> GetOfflinePlayerById(int id);
        public IPlayer GetOfflinePlayerByUsername(string username);
        public Task<IPlayer> CreatePlayer(int id, ISession session);
        public ValueTask RemovePlayer(int id);
        public ValueTask RemoveAllPlayers();
    }
}
