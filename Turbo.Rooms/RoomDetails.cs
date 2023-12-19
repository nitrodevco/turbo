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

        public bool UpdateSettingsForPlayer(IPlayer player, IRoomSettings message)
        {
            // fix this when updating multiple things at once and 1 fails

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

            Save();

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
            set
            {
                _roomEntity.Name = value;

                Save();
            }
        }

        public string Description
        {
            get => _roomEntity.Description == null ? "" : _roomEntity.Description;
            set
            {
                _roomEntity.Description = value;

                Save();
            }
        }

        public int PlayerId
        {
            get => _roomEntity.PlayerEntityId;
        }

        public string PlayerName { get; set; }

        public RoomStateType State
        {
            get => _roomEntity.RoomState;
            set
            {
                _roomEntity.RoomState = value;

                Save();
            }
        }

        public string Password
        {
            get => _roomEntity.Password;
            set
            {
                _roomEntity.Password = value;

                Save();
            }
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
            set
            {
                _roomEntity.UsersMax = value;

                Save();
            }
        }

        public double PaintWall
        {
            get => _roomEntity.PaintWall;
            set
            {
                _roomEntity.PaintWall = value;

                Save();
            }
        }

        public double PaintFloor
        {
            get => _roomEntity.PaintFloor;
            set
            {
                _roomEntity.PaintFloor = value;

                Save();
            }
        }

        public double PaintLandscape
        {
            get => _roomEntity.PaintLandscape;
            set
            {
                _roomEntity.PaintLandscape = value;

                Save();
            }
        }

        public int WallHeight
        {
            get => _roomEntity.WallHeight;
            set
            {
                _roomEntity.WallHeight = value;

                Save();
            }
        }

        public bool HideWalls
        {
            get => (bool)_roomEntity.HideWalls;
            set
            {
                _roomEntity.HideWalls = value;

                Save();
            }
        }

        public RoomThicknessType ThicknessWall
        {
            get => _roomEntity.ThicknessWall;
            set
            {
                _roomEntity.ThicknessWall = value;

                Save();
            }
        }

        public RoomThicknessType ThicknessFloor
        {
            get => _roomEntity.ThicknessFloor;
            set
            {
                _roomEntity.ThicknessFloor = value;

                Save();
            }
        }

        public bool BlockingDisabled
        {
            get => (bool)_roomEntity.AllowWalkThrough;
            set
            {
                _roomEntity.AllowWalkThrough = value;

                Save();
            }
        }

        public bool AllowEditing
        {
            get => (bool)_roomEntity.AllowEditing;
            set
            {
                _roomEntity.AllowEditing = value;

                Save();
            }
        }

        public bool AllowPets
        {
            get => (bool)_roomEntity.AllowPets;
            set
            {
                _roomEntity.AllowPets = value;

                Save();
            }
        }

        public bool AllowPetsEat
        {
            get => (bool)_roomEntity.AllowPetsEat;
            set
            {
                _roomEntity.AllowPetsEat = value;

                Save();
            }
        }

        public RoomTradeType TradeType
        {
            get => _roomEntity.TradeType;
            set
            {
                _roomEntity.TradeType = value;

                return;
            }
        }

        public RoomMuteType MuteType
        {
            get => _roomEntity.MuteType;
            set
            {
                _roomEntity.MuteType = value;

                Save();
            }
        }

        public RoomKickType KickType
        {
            get => _roomEntity.KickType;
            set
            {
                _roomEntity.KickType = value;

                Save();
            }
        }

        public RoomBanType BanType
        {
            get => _roomEntity.BanType;
            set
            {
                _roomEntity.BanType = value;

                Save();
            }
        }

        public RoomChatModeType ChatModeType
        {
            get => _roomEntity.ChatModeType;
            set
            {
                _roomEntity.ChatModeType = value;

                Save();
            }
        }

        public RoomChatWeightType ChatWeightType
        {
            get => _roomEntity.ChatWeightType;
            set
            {
                _roomEntity.ChatWeightType = value;

                Save();
            }
        }

        public RoomChatSpeedType ChatSpeedType
        {
            get => _roomEntity.ChatSpeedType;
            set
            {
                _roomEntity.ChatSpeedType = value;

                Save();
            }
        }

        public RoomChatProtectionType ChatProtectionType
        {
            get => _roomEntity.ChatProtectionType;
            set
            {
                _roomEntity.ChatProtectionType = value;

                Save();
            }
        }

        public int ChatDistance
        {
            get => _roomEntity.ChatDistance;
            set
            {
                _roomEntity.ChatDistance = value;

                Save();
            }
        }

        public DateTime LastActive
        {
            get => _roomEntity.LastActive;
            set
            {
                _roomEntity.LastActive = value;

                Save();
            }
        }
    }
}
