using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Core.Game.Rooms.Object.Logic
{
    public interface IMovingAvatarLogic : IRollingObjectLogic
    {
        public IPoint LocationNext { get; set; }
        public IPoint LocationGoal { get; }
        public IList<IPoint> CurrentPath { get; }
        public bool NeedsRepathing { get; set; }
        public bool IsWalking { get; }
        public bool CanWalk { get; }
        public void WalkTo(IPoint location);
        public void StopWalking();
        public void ClearPath();
        public void ProcessNextLocation();
        public void UpdateHeight(IRoomTile roomTile = null);
        public void InvokeCurrentLocation();
        public void AddStatus(string type, string value);
        public bool HasStatus(params string[] types);
        public void RemoveStatus(params string[] types);
        public IRoomTile GetNextTile();
    }
}
