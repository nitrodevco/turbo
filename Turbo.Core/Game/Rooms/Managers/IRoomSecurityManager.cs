using System;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Packets.Messages;

namespace Turbo.Core.Game.Rooms.Managers
{
    public interface IRoomSecurityManager : IAsyncInitialisable, IAsyncDisposable
    {
        public bool IsOwner(IRoomManipulator manipulator);
        public bool IsStrictOwner(IRoomManipulator manipulator);
        public bool IsController(IRoomManipulator manipulator);
        public void RefreshControllerLevel(IRoomObject roomObject);
        public void SendOwnersComposer(IComposer composer);
        public void SendRightsComposer(IComposer composer);
    }
}
