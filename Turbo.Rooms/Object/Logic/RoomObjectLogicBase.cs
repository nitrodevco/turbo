using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic;

namespace Turbo.Rooms.Object.Logic
{
    public class RoomObjectLogicBase : IRoomObjectLogic
    {
        public IRoomObject RoomObject { get; private set; }

        public RoomObjectLogicBase()
        {

        }

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

        public bool SetRoomObject(IRoomObject roomObject)
        {
            if (roomObject == RoomObject) return true;

            if (RoomObject != null)
            {
                RoomObject.SetLogic(null);
            }

            if (roomObject == null)
            {
                Dispose();

                RoomObject = null;

                return false;
            }

            RoomObject = roomObject;

            RoomObject.SetLogic(this);

            return true;
        }

        public virtual async Task Cycle()
        {

        }
    }
}
