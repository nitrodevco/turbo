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

        public readonly RoomSecurityManager RoomSecurityManager;
        public readonly RoomFurnitureManager RoomFurnitureManager;
        public readonly RoomUserManager RoomUserManager;

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

        public async ValueTask InitAsync()
        {
            await LoadMapping();

            if (RoomSecurityManager != null) await RoomSecurityManager.InitAsync();
            if (RoomFurnitureManager != null) await RoomFurnitureManager.InitAsync();
            if (RoomUserManager != null) await RoomUserManager.InitAsync();
        }

        public async ValueTask DisposeAsync()
        {
            if (IsDisposing) return;

            IsDisposing = true;

            CancelDispose();

            if(RoomManager != null)
            {
                await RoomManager.RemoveRoom(Id);
            }

            if (RoomUserManager != null) await RoomUserManager.DisposeAsync();
            if (RoomFurnitureManager != null) await RoomFurnitureManager.DisposeAsync();
            if (RoomSecurityManager != null) await RoomSecurityManager.DisposeAsync();
        }

        public void TryDispose()
        {

        }

        public void CancelDispose()
        {

        }

        private async ValueTask LoadMapping()
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
