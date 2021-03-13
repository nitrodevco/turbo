using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Messages;
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

        public void Dispose()
        {

        }

        public bool SetRoomObject(IRoomObject roomObject)
        {
            if (roomObject == RoomObject) return true;

            if(RoomObject != null)
            {
                RoomObject.SetLogic(null);
            }

            if(roomObject == null)
            {
                Dispose();

                RoomObject = null;

                return false;
            }

            RoomObject = roomObject;

            RoomObject.SetLogic(this);

            return true;
        }

        public virtual void ProcessUpdateMessage(RoomObjectUpdateMessage updateMessage)
        {
            if ((updateMessage == null) || (RoomObject == null)) return;

            RoomObject.SetLocation(updateMessage.Location);
        }
    }
}
