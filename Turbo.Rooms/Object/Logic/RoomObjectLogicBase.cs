using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Rooms.Object.Logic
{
    public abstract class RoomObjectLogicBase : IRoomObjectLogic
    {
        public virtual void Dispose()
        {
            CleanUp();
        }

        protected virtual void CleanUp()
        {

        }

        public virtual bool OnReady()
        {
            return true;
        }

        public virtual async Task Cycle()
        {

        }
    }
}
