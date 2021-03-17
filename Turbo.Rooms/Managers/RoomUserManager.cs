using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Packets.Messages;
using Turbo.Packets.Outgoing.Navigator;
using Turbo.Packets.Outgoing.Room.Engine;
using Turbo.Packets.Outgoing.Room.Session;
using Turbo.Rooms.Object;
using Turbo.Rooms.Object.Logic.Avatar;

namespace Turbo.Rooms.Managers
{
    public class RoomUserManager : IRoomUserManager
    {
        private IRoom _room;

        private readonly IList<ISession> _roomObservers;
        private int _roomObjectCounter;

        public IDictionary<int, IRoomObject> RoomObjects { get; private set; }

        public RoomUserManager(IRoom room)
        {

            _roomObservers = new List<ISession>();
            _roomObjectCounter = -1;
            _room = room;

            RoomObjects = new Dictionary<int, IRoomObject>();
        }

        public async ValueTask InitAsync()
        {

        }

        public async ValueTask DisposeAsync()
        {

        }

        public IRoomObject GetRoomObject(int id)
        {
            if (id < 0) return null;

            IRoomObject roomObject;

            if (RoomObjects.TryGetValue(id, out roomObject))
            {
                return roomObject;
            }

            return null;
        }

        public IRoomObject GetRoomObjectByUserId(int userId)
        {
            return null;
        }

        public IRoomObject GetRoomObjectByUsername(string username)
        {
            return null;
        }

        public IRoomObject AddRoomObject(IRoomObject roomObject, IPoint location = null)
        {
            if (roomObject == null) return null;

            IRoomObject existingRoomObject = GetRoomObject(roomObject.Id);

            if(existingRoomObject != null)
            {
                roomObject.Dispose();

                return null;
            }

            if (roomObject.Logic is not MovingAvatarLogic avatarLogic) return null;

            if (location == null) location = _room.RoomModel.DoorLocation.Clone();

            roomObject.SetLocation(location);

            IRoomTile roomTile = avatarLogic.GetCurrentTile();

            if (roomTile != null) roomTile.AddRoomObject(roomObject);

            avatarLogic.InvokeCurrentLocation();

            roomObject.NeedsUpdate = false;

            RoomObjects.Add(roomObject.Id, roomObject);

            SendComposer(new UsersMessage
            {

            });

            SendComposer(new UserUpdateMessage
            {

            });

            UpdateTotalUsers();

            return roomObject;
        }

        public IRoomObject CreateRoomObjectAndAssign(IRoomObjectFactory objectFactory, IRoomObjectHolder roomObjectHolder, IPoint location)
        {
            if (roomObjectHolder == null) return null;

            IRoomObject roomObject = objectFactory.CreateRoomObject(_room, _roomObjectCounter++, roomObjectHolder.Type, roomObjectHolder.Type);

            if (roomObject == null) return null;

            if (!roomObjectHolder.SetRoomObject(roomObject)) return null;

            return AddRoomObject(roomObject, location);
        }

        public void RemoveRoomObject(int id)
        {
            IRoomObject roomObject = GetRoomObject(id);

            if (roomObject == null) return;

            SendComposer(new UserRemoveMessage
            {
                Id = id
            });

            RoomObjects.Remove(id);

            // if the room object was playing a game, remove it from that game

            UpdateTotalUsers();

            roomObject.Dispose();

            _room.TryDispose();
        }

        public void RemoveAllRoomObjects()
        {
            foreach (int id in RoomObjects.Keys) RemoveRoomObject(id);
        }

        public void EnterRoom(IRoomObjectFactory objectFactory, IPlayer player, IPoint location = null)
        {
            if ((objectFactory == null) || (player == null)) return;

            // set the pending room id

            IRoomObject roomObject = CreateRoomObjectAndAssign(objectFactory, player, location);

            if(roomObject == null)
            {
                player.Session.Send(new CantConnectMessage
                {
                    Reason = CantConnectReason.Closed
                });

                return;
            }

            player.Session.SendQueue(new HeightMapMessage
            {
                RoomModel = _room.RoomModel,
                RoomMap = _room.RoomMap
            });

            player.Session.SendQueue(new FloorHeightMapMessage
            {
                IsZoomedIn = true,
                WallHeight = 1,
                RoomModel = _room.RoomModel
            });

            player.Session.SendQueue(new RoomVisualizationSettingsMessage
            {
                WallsHidden = false,
                FloorThickness = 1,
                WallThickness = 1
            });

            // would be nice to send this from the navigator so we aren't duplicating code
            player.Session.SendQueue(new GetGuestRoomResultMessage
            {
                EnterRoom = false,
                Room = _room,
                IsRoomForward = false,
                IsStaffPick = false,
                IsGroupMember = false,
                AllInRoomMuted = false,
                CanMute = false
            });

            player.Session.Flush();
        }

        private void UpdateTotalUsers()
        {

        }

        public void SendComposer(IComposer composer)
        {
            foreach(ISession session in _roomObservers)
            {
                session.Send(composer);
            }
        }
    }
}
