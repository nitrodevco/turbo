using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Rooms.Managers;
using Turbo.Rooms.Mapping;

namespace Turbo.Rooms
{
    public class Room : IRoom
    {
        private readonly IRoomManager _roomManager;

        public IRoomDetails RoomDetails { get; private set; }

        private readonly IRoomSecurityManager _roomSecurityManager;
        private readonly IRoomFurnitureManager _roomFurnitureManager;
        private readonly IRoomUserManager _roomUserManager;
        private readonly ILogger<IRoom> _logger;

        public IRoomModel RoomModel { get; private set; }
        public IRoomMap RoomMap { get; private set; }

        public bool IsDisposed { get; private set; }
        public bool IsDisposing { get; private set; }

        public Room(IRoomManager roomManager, ILogger<IRoom> logger, IRoomSecurityManager securityManager, 
            IRoomFurnitureManager furnitureManager, IRoomUserManager roomUserManager, IRoomDetails roomDetails)
        {
            _roomManager = roomManager;
            _logger = logger;

            RoomDetails = roomDetails;

            _roomSecurityManager = securityManager;
            _roomFurnitureManager = furnitureManager;
            _roomUserManager = roomUserManager;
        }

        public async ValueTask InitAsync()
        {
            await LoadMapping();

            if (_roomSecurityManager != null) await _roomSecurityManager.InitAsync();
            if (_roomFurnitureManager != null) await _roomFurnitureManager.InitAsync();
            if (_roomUserManager != null) await _roomUserManager.InitAsync();
        }

        public async ValueTask DisposeAsync()
        {
            if (IsDisposing) return;

            IsDisposing = true;

            CancelDispose();

            if (_roomManager != null)
            {
                await _roomManager.RemoveRoom(Id);
            }

            if (_roomUserManager != null) await _roomUserManager.DisposeAsync();
            if (_roomFurnitureManager != null) await _roomFurnitureManager.DisposeAsync();
            if (_roomSecurityManager != null) await _roomSecurityManager.DisposeAsync();
        }

        public void TryDispose()
        {
            if (IsDisposed || IsDisposing) return;

            // if dispose already scheduled, return

            // if has users, return

            // clear the users waiting at the door

            // schedule the dispose to run in 1 minute
        }

        public void CancelDispose()
        {
            // if dispose already scheduled, cancel it
        }

        private async ValueTask LoadMapping()
        {
            if (RoomMap != null)
            {
                RoomMap.Dispose();

                RoomMap = null;
            }

            RoomModel = null;

            IRoomModel roomModel = _roomManager.GetModel(RoomDetails.ModelId);

            if ((roomModel == null) || (!roomModel.DidGenerate)) return;

            RoomModel = roomModel;
            RoomMap = new RoomMap(this);

            RoomMap.GenerateMap();
        }

        public Task Cycle()
        {
            throw new System.NotImplementedException();
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
