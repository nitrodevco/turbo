using System;
using Turbo.Core.Networking.Game.Clients;

namespace Turbo.Core.Game.Players
{
    public interface IPlayer : IAsyncInitialisable, IAsyncDisposable
    {
        public IPlayerDetails PlayerDetails { get; }

        public bool SetSession(ISession session);

        public int Id { get; }
        public string Name { get; }
    }
}
