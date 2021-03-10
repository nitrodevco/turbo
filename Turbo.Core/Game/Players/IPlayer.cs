using System;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Networking.Game.Clients;

namespace Turbo.Core.Game.Players
{
    public interface IPlayer : IRoomObjectHolder, IAsyncInitialisable, IAsyncDisposable
    {
        public ISession Session { get; }
        public IPlayerDetails PlayerDetails { get; }

        public bool SetSession(ISession session);

        public int Id { get; }
        public string Name { get; }
    }
}
