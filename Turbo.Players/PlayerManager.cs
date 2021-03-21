using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Navigator;
using Turbo.Core.Game.Players;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Database.Entities.Players;
using Turbo.Database.Repositories.Player;
using Turbo.Players.Factories;

namespace Turbo.Players
{
    public class PlayerManager : IPlayerManager, IPlayerContainer
    {
        private readonly IPlayerFactory _playerFactory;
        private readonly INavigatorManager _navigatorManager;
        private readonly ILogger<PlayerManager> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly IDictionary<int, IPlayer> _players;

        public PlayerManager(
            IPlayerFactory playerFactory,
            IServiceScopeFactory scopeFactory,
            INavigatorManager navigatorManager,
            ILogger<PlayerManager> logger)
        {
            _playerFactory = playerFactory;
            _logger = logger;
            _navigatorManager = navigatorManager;
            _serviceScopeFactory = scopeFactory;

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

            PlayerEntity playerEntity;
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var playerRepository = scope.ServiceProvider.GetService<IPlayerRepository>();
                playerEntity = await playerRepository.FindAsync(id);
            }

            if (playerEntity == null) return null;

            player = _playerFactory.Create(this, playerEntity);

            return player;
        }

        public async Task<IPlayer> GetOfflinePlayerByUsername(string username)
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

        public void ClearPlayerRoomStatus(IPlayer player)
        {
            if (player == null) return;

            _navigatorManager.ClearRoomStatus(player);
        }
    }
}
