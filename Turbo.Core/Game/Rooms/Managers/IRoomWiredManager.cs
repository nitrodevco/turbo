using Turbo.Core.Game.Wired;

namespace Turbo.Core.Game.Rooms.Managers
{
    public interface IRoomWiredManager
    {
        public bool ProcessTriggers(string type, IWiredArguments wiredArguments = null);
    }
}
