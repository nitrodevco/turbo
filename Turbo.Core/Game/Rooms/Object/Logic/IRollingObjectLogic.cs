using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Core.Game.Rooms.Object.Logic
{
    public interface IRollingObjectLogic : IRoomObjectLogic
    {
        public IRollerData RollerData { get; set; }
        public bool IsRolling { get; }
    }
}
