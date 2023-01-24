using System.Collections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Navigator;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Storage;
using Turbo.Database.Entities.Players;
using Turbo.Database.Repositories.Player;
using Turbo.Players.Factories;
using Turbo.Core.Game.Inventory;
using Turbo.Core.Database.Dtos;

namespace Turbo.Players
{
    public class PlayerManager : IPlayerManager
    {
        private readonly ILogger<IPlayerManager> _logger;
        private readonly IStorageQueue _storageQueue;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IPlayerFactory _playerFactory;
        private readonly INavigatorManager _navigatorManager;

        private readonly IDictionary<int, IPlayer> _players;

        public PlayerManager(
            ILogger<IPlayerManager> logger,
            IStorageQueue storageQueue,
            IServiceScopeFactory scopeFactory,
            IPlayerFactory playerFactory,
            INavigatorManager navigatorManager)
        {
            _logger = logger;
            _storageQueue = storageQueue;
            _serviceScopeFactory = scopeFactory;
            _playerFactory = playerFactory;
            _navigatorManager = navigatorManager;

            _players = new ConcurrentDictionary<int, IPlayer>();
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

            player = _playerFactory.Create(playerEntity);

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

        public async Task EnterRoom(IPlayer player, int roomId, string password = null, bool skipState = false, IPoint location = null)
        {
            await _navigatorManager.EnterRoom(player, roomId, password, skipState, location);
        }

        public async Task<IList<IPlayerBadge>> GetPlayerActiveBadges(int playerId)
        {
            if (playerId <= 0) return null;

            var player = GetPlayerById(playerId);

            if (player == null)
            {
                List<PlayerBadgeDto> entities = new();

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var playerBadgeRepository = scope.ServiceProvider.GetService<IPlayerBadgeRepository>();
                    entities = await playerBadgeRepository.FindActiveByPlayerIdAsync(playerId);
                }

                return (IList<IPlayerBadge>)entities;
            }

            return player.PlayerInventory?.BadgeInventory?.ActiveBadges;
        }

        public IStorageQueue StorageQueue => _storageQueue;
    }
}
