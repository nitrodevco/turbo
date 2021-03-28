using System;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Packets.Messages;

namespace Turbo.Core.Game.Rooms.Managers
{
    public interface IRoomSecurityManager : IAsyncInitialisable, IAsyncDisposable
    {
        public bool IsOwner(IPlayer player);
        public bool IsStrictOwner(IPlayer player);
        public bool IsController(IPlayer player);
        public void RefreshControllerLevel(IRoomObject roomObject);
        public void SendOwnersComposer(IComposer composer);
        public void SendRightsComposer(IComposer composer);
    }
}
