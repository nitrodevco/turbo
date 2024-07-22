using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Core.Game.Rooms.Object.Logic;

public interface IRollingObjectLogic
{
    public IRollerData RollerData { get; set; }
    public bool IsRolling { get; }
}