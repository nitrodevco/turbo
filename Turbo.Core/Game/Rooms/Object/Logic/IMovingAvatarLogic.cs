using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Core.Game.Rooms.Object.Logic
{
    public interface IMovingAvatarLogic
    {
        public void WalkTo(IPoint location);
        public void StopWalking();
        public void ClearPath();
        public void ProcessNextLocation();
        public void UpdateHeight(IRoomTile roomTile = null);
        public void InvokeCurrentLocation();
        public IRoomTile GetCurrentTile();
        public IRoomTile GetNextTile();
    }
}
