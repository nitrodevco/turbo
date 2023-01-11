using System;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Packets.Messages;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Furniture.Constants;
using Turbo.Core.Game.Rooms.Constants;

namespace Turbo.Core.Game.Rooms.Managers
{
    public interface IRoomSecurityManager : IAsyncInitialisable, IAsyncDisposable
    {
        public bool IsStrictOwner(IRoomManipulator manipulator);
        public bool IsOwner(IRoomManipulator manipulator);
        public RoomControllerLevel GetControllerLevel(IRoomManipulator manipulator);
        public void RefreshControllerLevel(IRoomObjectAvatar avatarObject);
        public void SendOwnersComposer(IComposer composer);
        public void SendRightsComposer(IComposer composer);
        public bool CanManipulateFurniture(IRoomManipulator manipulator, IRoomFurniture furniture);
        public bool CanPlaceFurniture(IRoomManipulator manipulator);
        public FurniturePickupType GetFurniturePickupType(IRoomManipulator manipulator, IRoomFurniture furniture);
    }
}
