using System;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Constants;
using Turbo.Database.Entities.Room;

namespace Turbo.Rooms
{
    public class RoomDetails : IRoomDetails
    {
        private readonly IRoom _room;
        private readonly RoomEntity _roomEntity;

        public RoomDetails(IRoom room, RoomEntity roomEntity)
        {
            _room = room;
            _roomEntity = roomEntity;
        }

        public void Save()
        {
            _room.RoomManager.StorageQueue.Add(_roomEntity);
        }

        public int Id => _roomEntity.Id;

        public string Name
        {
            get => _roomEntity.Name;
        }

        public string Description
        {
            get => _roomEntity.Description == null ? "" : _roomEntity.Description;
        }

        public int PlayerId
        {
            get => _roomEntity.PlayerEntityId;
        }

        public string PlayerName { get; set; }

        public RoomStateType State
        {
            get => _roomEntity.RoomState;
        }

        public string Password
        {
            get => _roomEntity.Password;
        }

        public int ModelId => _roomEntity.RoomModelEntityId;

        public int UsersNow
        {
            get => _roomEntity.UsersNow;
            set
            {
                if (_roomEntity.UsersNow == value) return;

                _roomEntity.UsersNow = value;

                Save();
            }
        }

        public int UsersMax
        {
            get => _roomEntity.UsersMax;
        }

        public double PaintWall
        {
            get => _roomEntity.PaintWall;
        }

        public double PaintFloor
        {
            get => _roomEntity.PaintFloor;
        }

        public double PaintLandscape
        {
            get => _roomEntity.PaintLandscape;
        }

        public int WallHeight
        {
            get => _roomEntity.WallHeight;
        }

        public bool HideWalls
        {
            get => (bool)_roomEntity.HideWalls;
        }

        public RoomThicknessType ThicknessWall
        {
            get => _roomEntity.ThicknessWall;
        }

        public RoomThicknessType ThicknessFloor
        {
            get => _roomEntity.ThicknessFloor;
        }

        public bool AllowWalkThrough
        {
            get => (bool)_roomEntity.AllowWalkThrough;
        }

        public bool AllowEditing
        {
            get => (bool)_roomEntity.AllowEditing;
        }

        public bool AllowPets
        {
            get => (bool)_roomEntity.AllowPets;
        }

        public bool AllowPetsEat
        {
            get => (bool)_roomEntity.AllowPetsEat;
        }

        public RoomTradeType TradeType
        {
            get => _roomEntity.TradeType;
        }

        public RoomMuteType MuteType
        {
            get => _roomEntity.MuteType;
        }

        public RoomKickType KickType
        {
            get => _roomEntity.KickType;
        }

        public RoomBanType BanType
        {
            get => _roomEntity.BanType;
        }

        public RoomChatModeType ChatModeType
        {
            get => _roomEntity.ChatModeType;
        }

        public RoomChatWeightType ChatWeightType
        {
            get => _roomEntity.ChatWeightType;
        }

        public RoomChatSpeedType ChatSpeedType
        {
            get => _roomEntity.ChatSpeedType;
        }

        public RoomChatProtectionType ChatProtectionType
        {
            get => _roomEntity.ChatProtectionType;
        }

        public int ChatDistance
        {
            get => _roomEntity.ChatDistance;
        }

        public DateTime LastActive
        {
            get => _roomEntity.LastActive;
        }
    }
}
