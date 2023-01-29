using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Turbo.Core.Game;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Constants;
using Turbo.Database.Entities.Room;
using Turbo.Packets.Incoming.RoomSettings;
using Turbo.Packets.Outgoing.Navigator;
using Turbo.Packets.Outgoing.Room.Chat;
using Turbo.Packets.Outgoing.Room.Engine;
using Turbo.Packets.Outgoing.RoomSettings;

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

        public async Task SaveNow()
        {
            await _room.RoomManager.StorageQueue.SaveNow(_roomEntity);
        }

        public async Task<bool> UpdateSettingsForPlayer(IPlayer player, IRoomSettings message)
        {
            if (!message.Name.Equals(Name))
            {
                if (DefaultSettings.RoomNameRegex.IsMatch(message.Name))
                {
                    // check word filter too
                    _roomEntity.Name = message.Name;
                }
                else
                {
                    SendSettingsSaveErrorToPlayer(player, RoomSettingsErrorType.InvalidName);

                    return false;
                }
            }

            if (!message.Description.Equals(Description))
            {
                if (DefaultSettings.RoomDescriptionRegex.IsMatch(message.Name))
                {
                    // check word filter too
                    _roomEntity.Description = message.Description;
                }
                else
                {
                    SendSettingsSaveErrorToPlayer(player, RoomSettingsErrorType.InvalidDescription);

                    return false;
                }
            }

            if (message.State != State)
            {
                if (Enum.IsDefined(typeof(RoomStateType), message.State))
                {
                    if (message.State == RoomStateType.Password)
                    {
                        if (!DefaultSettings.RoomPasswordRegex.IsMatch(message.Password))
                        {
                            SendSettingsSaveErrorToPlayer(player, RoomSettingsErrorType.InvalidPassword);

                            return false;
                        }

                        _roomEntity.Password = message.Password;
                    }

                    _roomEntity.RoomState = message.State;
                }
                else
                {
                    SendSettingsSaveErrorToPlayer(player, RoomSettingsErrorType.InvalidDoorMode);

                    return false;
                }
            }

            var usersMax = Math.Max(1, Math.Min(message.UsersMax, DefaultSettings.MaximumUsersPerRoom));

            if (usersMax != UsersMax)
            {
                _roomEntity.UsersMax = usersMax;
            }

            if (message.TradeType != TradeType && Enum.IsDefined(typeof(RoomTradeType), message.TradeType))
            {
                _roomEntity.TradeType = message.TradeType;
            }

            _roomEntity.AllowPets = message.AllowPets;
            _roomEntity.AllowPetsEat = message.AllowPetsEat;
            _roomEntity.AllowWalkThrough = message.BlockingDisabled;

            UpdateRoomVisualization(message.HideWalls, message.WallThickness, message.FloorThickness);
            UpdateRoomModeration(message.MuteType, message.KickType, message.BanType);
            UpdateRoomChat(message.ChatModeType, message.ChatWeightType, message.ChatSpeed, message.ChatDistance, message.ChatProtectionType);

            await SaveNow();

            _room.SendComposer(new RoomInfoUpdatedMessage
            {
                RoomId = Id
            });

            return true;
        }

        private void SendSettingsSaveErrorToPlayer(IPlayer player, RoomSettingsErrorType errorType)
        {
            player.Session.Send(new RoomSettingsSaveErrorMessage
            {
                RoomId = Id,
                ErrorCode = errorType,
                Info = ""
            });
        }

        public void UpdateRoomVisualization(bool hideWalls, RoomThicknessType wallThickness, RoomThicknessType floorThickness)
        {
            var updated = false;

            if (hideWalls != HideWalls)
            {
                updated = true;

                _roomEntity.HideWalls = hideWalls;
            }

            if (wallThickness != ThicknessWall && Enum.IsDefined(typeof(RoomThicknessType), wallThickness))
            {
                updated = true;

                _roomEntity.ThicknessWall = wallThickness;
            }

            if (floorThickness != ThicknessFloor && Enum.IsDefined(typeof(RoomThicknessType), floorThickness))
            {
                updated = true;

                _roomEntity.ThicknessFloor = floorThickness;
            }

            if (!updated) return;

            _room.SendComposer(new RoomVisualizationSettingsMessage
            {
                WallsHidden = hideWalls,
                WallThickness = (int)wallThickness,
                FloorThickness = (int)floorThickness
            });

            Save();
        }

        public void UpdateRoomModeration(RoomMuteType muteType, RoomKickType kickType, RoomBanType banType)
        {
            var updated = false;

            if (muteType != MuteType && Enum.IsDefined(typeof(RoomMuteType), muteType))
            {
                updated = true;

                _roomEntity.MuteType = muteType;
            }

            if (kickType != KickType && Enum.IsDefined(typeof(RoomKickType), kickType))
            {
                updated = true;

                _roomEntity.KickType = kickType;
            }

            if (banType != BanType && Enum.IsDefined(typeof(RoomBanType), banType))
            {
                updated = true;

                _roomEntity.BanType = banType;
            }

            if (!updated) return;

            Save();
        }

        public void UpdateRoomChat(RoomChatModeType modeType, RoomChatWeightType weightType, RoomChatSpeedType speedType, int distance, RoomChatProtectionType protectionType)
        {
            var updated = false;

            if (modeType != ChatModeType && Enum.IsDefined(typeof(RoomChatModeType), modeType))
            {
                updated = true;

                _roomEntity.ChatModeType = modeType;
            }

            if (weightType != ChatWeightType && Enum.IsDefined(typeof(RoomChatWeightType), weightType))
            {
                updated = true;

                _roomEntity.ChatWeightType = weightType;
            }

            if (speedType != ChatSpeedType && Enum.IsDefined(typeof(RoomChatSpeedType), speedType))
            {
                updated = true;

                _roomEntity.ChatSpeedType = speedType;
            }

            distance = Math.Min(distance, DefaultSettings.MaximumChatDistance);

            if (distance != ChatDistance)
            {
                updated = true;

                _roomEntity.ChatDistance = distance;
            }

            if (protectionType != ChatProtectionType && Enum.IsDefined(typeof(RoomChatProtectionType), protectionType))
            {
                updated = true;

                _roomEntity.ChatProtectionType = protectionType;
            }

            if (!updated) return;

            _room.SendComposer(new RoomChatSettingsMessage
            {
                ChatMode = _roomEntity.ChatModeType,
                ChatWeight = _roomEntity.ChatWeightType,
                ChatSpeed = _roomEntity.ChatSpeedType,
                ChatDistance = _roomEntity.ChatDistance,
                ChatProtection = _roomEntity.ChatProtectionType
            });

            Save();
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

        public bool BlockingDisabled
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
