using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic.Wired;

namespace Turbo.Rooms.Object.Logic.Furniture.Wired.Arguments
{
    public class WiredArguments : IWiredArguments
    {
        public IRoomObjectAvatar UserObject { get; set; }
        public IRoomObjectFloor FurnitureObject { get; set; }
    }
}
