using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Rooms.Managers;
using Turbo.Rooms.Mapping;

namespace Turbo.Rooms
{
    public class Room : IRoom
    {
        public IRoomManager RoomManager { get; private set; }
        public RoomDetails RoomDetails { get; private set; }

        public RoomSecurityManager RoomSecurityManager { get; private set; }
        public RoomFurnitureManager RoomFurnitureManager { get; private set; }
        public RoomUserManager RoomUserManager { get; private set; }

        public IRoomModel RoomModel { get; private set; }
        public IRoomMap RoomMap { get; private set; }

        public bool IsDisposing { get; private set; }

        public Room(IRoomManager roomManager, RoomDetails roomDetails)
        {
            RoomManager = roomManager;
            RoomDetails = roomDetails;

            RoomSecurityManager = new RoomSecurityManager(this);
            RoomFurnitureManager = new RoomFurnitureManager(this);
            RoomUserManager = new RoomUserManager(this);
        }

        public void Init()
        {
            LoadMapping();

            if (RoomSecurityManager != null) RoomSecurityManager.Init();
            if (RoomFurnitureManager != null) RoomFurnitureManager.Init();
            if (RoomUserManager != null) RoomUserManager.Init();
        }

        public void Dispose()
        {
            if (IsDisposing) return;

            IsDisposing = true;

            CancelDispose();

            if(RoomManager != null)
            {
                RoomManager.RemoveRoom(Id);
            }

            if (RoomUserManager != null) RoomUserManager.Dispose();
            if (RoomFurnitureManager != null) RoomFurnitureManager.Dispose();
            if (RoomSecurityManager != null) RoomSecurityManager.Dispose();
        }

        public void TryDispose()
        {

        }

        public void CancelDispose()
        {

        }

        private void LoadMapping()
        {
            if(RoomMap != null)
            {
                RoomMap.Dispose();

                RoomMap = null;
            }

            RoomModel = null;
        }

        public int Id
        {
            get
            {
                return RoomDetails.Id;
            }
        }
    }
}
