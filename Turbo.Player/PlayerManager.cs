using System;
using System.Collections.Generic;
using System.Text;
using Turbo.Packets.Sessions;

namespace Turbo.Players
{
    public class PlayerManager : IPlayerManager, IPlayerContainer
    {
        internal readonly Dictionary<int, IPlayer> _players;

        public PlayerManager()
        {
            _players = new Dictionary<int, IPlayer>();
        }

        public void Dispose()
        {

        }

        public IPlayer GetPlayerById(int id)
        {
            if ((id <= 0)) return null;

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

        public IPlayer GetOfflinePlayerById(int id)
        {
            IPlayer player = GetPlayerById(id);

            if (player != null) return player;

            // lookup in repository
            // create new Player(this, PlayerEntity);

            return null;
        }

        public IPlayer GetOfflinePlayerByUsername(string username)
        {
            IPlayer player = GetPlayerByUsername(username);

            if (player != null) return player;

            // lookup in repository
            // create new Player(this, PlayerEntity);

            return null;
        }

        public IPlayer CreatePlayer(int id, ISession session)
        {
            if ((id <= 0) || (session == null)) return null;

            IPlayer player = GetOfflinePlayerById(id);

            if (player == null) return null;

            if(!player.SetSession(session))
            {
                player.Dispose();
                session.Dispose();

                return null;
            }

            _players.Add(id, player);

            player.Init();

            return player;
        }

        public void RemovePlayer(int id)
        {
            if (id <= 0) return;

            IPlayer player = GetPlayerById(id);

            if (player == null) return;

            _players.Remove(id);

            player.Dispose();
        }

        public void RemoveAllPlayers()
        {
            foreach(IPlayer player in _players.Values)
            {
                _players.Remove(player.Id);

                player.Dispose();
            }
        }
    }
}
