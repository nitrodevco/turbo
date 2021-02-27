using Turbo.Core;
using System;
using Turbo.Packets.Sessions;

namespace Turbo.Players
{
    public interface IPlayer : ISessionPlayer, IAsyncInitialisable, IAsyncDisposable
    {
        public PlayerDetails PlayerDetails { get; }

        public bool SetSession(ISession session);

        public int Id { get; }
        public string Name { get; }
    }
}
