using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Database.Entities.Players;
using Turbo.Database.Repositories.Player;
using Turbo.Packets.Sessions;

namespace Turbo.Players
{
    public class PlayerManager : IPlayerManager, IPlayerContainer
    {
        private readonly IPlayerRepository _playerRepository;

        private readonly Dictionary<int, IPlayer> _players;

        public PlayerManager(IPlayerRepository playerRepository, ILogger<IPlayerManager> logger)
        {
            _playerRepository = playerRepository;

            _players = new Dictionary<int, IPlayer>();
        }

        public async ValueTask DisposeAsync()
        {
            await RemoveAllPlayers();
        }

        public IPlayer GetPlayerById(int id)
        {
            if ((id <= 0) || !_players.ContainsKey(id)) return null;

            return _players[id];
        }

        public IPlayer GetPlayerByUsername(string username)
        {
            if (username.Length == 0) return null;

            foreach (IPlayer player in _players.Values)
            {
                if ((player == null) || !player.Name.Equals(username)) continue;

                return player;
            }

            return null;
        }

        public async Task<IPlayer> GetOfflinePlayerById(int id)
        {
            IPlayer player = GetPlayerById(id);

            if (player != null) return player;

            PlayerEntity playerEntity = await _playerRepository.FindAsync(id);

            if (playerEntity == null) return null;

            player = new Player(this, playerEntity);

            return player;
        }

        public IPlayer GetOfflinePlayerByUsername(string username)
        {
            IPlayer player = GetPlayerByUsername(username);

            if (player != null) return player;

            // lookup in repository
            // create new Player(this, PlayerEntity);

            return null;
        }

        public async Task<IPlayer> CreatePlayer(int id, ISession session)
        {
            if ((id <= 0) || (session == null)) return null;

            IPlayer player = await GetOfflinePlayerById(id);

            if (player == null) return null;

            if (!player.SetSession(session))
            {
                await player.DisposeAsync();
                await session.DisposeAsync();

                return null;
            }

            _players.Add(id, player);

            await player.InitAsync();

            return player;
        }

        public async ValueTask RemovePlayer(int id)
        {
            if (id <= 0) return;

            IPlayer player = GetPlayerById(id);

            if (player == null) return;

            _players.Remove(id);

            await player.DisposeAsync();
        }

        public async ValueTask RemoveAllPlayers()
        {
            foreach (int id in _players.Keys)
            {
                await RemovePlayer(id);
            }
        }
    }
}
