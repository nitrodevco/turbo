using System;
using Turbo.Core.Game;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Constants;
using Turbo.Core.Storage;
using Turbo.Database.Entities.Room;
using Turbo.Packets.Outgoing.Navigator;
using Turbo.Packets.Outgoing.Room.Chat;
using Turbo.Packets.Outgoing.Room.Engine;
using Turbo.Packets.Outgoing.RoomSettings;

namespace Turbo.Rooms;

public class RoomDetails(
    IRoom _room,
    RoomEntity _roomEntity,
    IStorageQueue _storageQueue) : IRoomDetails
{
    public bool UpdateSettingsForPlayer(IPlayer player, IRoomSettings message)
    {
        // fix this when updating multiple things at once and 1 fails

        if (!message.Name.Equals(Name))
        {
            if (DefaultSettings.RoomNameRegex.IsMatch(message.Name))
            {
                // TODO pass this through the word filter
                _roomEntity.Name = message.Name;
            }
            else
            {
                // TODO if the wordfilter doesnt like the name, send BadName
                SendSettingsSaveErrorToPlayer(player, RoomSettingsErrorType.InvalidName);

                return false;
            }
        }

        if (!message.Description.Equals(Description))
        {
            if (DefaultSettings.RoomDescriptionRegex.IsMatch(message.Name))
            {
                // TODO pass this through the word filter
                _roomEntity.Description = message.Description;
            }
            else
            {
                // TODO if the wordfilter doesnt like the description, send BadDescription
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

        if (usersMax != UsersMax) _roomEntity.UsersMax = usersMax;

        if (message.TradeType != TradeType && Enum.IsDefined(typeof(RoomTradeType), message.TradeType))
            _roomEntity.TradeType = message.TradeType;

        _roomEntity.AllowPets = message.AllowPets;
        _roomEntity.AllowPetsEat = message.AllowPetsEat;
        _roomEntity.AllowWalkThrough = message.BlockingDisabled;

        UpdateRoomVisualization(message.HideWalls, message.WallThickness, message.FloorThickness);
        UpdateRoomModeration(message.MuteType, message.KickType, message.BanType);
        UpdateRoomChat(message.ChatModeType, message.ChatWeightType, message.ChatSpeed, message.ChatDistance,
            message.ChatProtectionType);

        _storageQueue.Add(_roomEntity);

        _room.SendComposer(new RoomInfoUpdatedMessage
        {
            RoomId = Id
        });

        return true;
    }

    public int Id => _roomEntity.Id;

    public string Name
    {
        get => _roomEntity.Name;
        set
        {
            _roomEntity.Name = value;
            _storageQueue.Add(_roomEntity);
        }
    }

    public string Description
    {
        get => _roomEntity.Description == null ? "" : _roomEntity.Description;
        set
        {
            _roomEntity.Description = value;
            _storageQueue.Add(_roomEntity);
        }
    }

    public int PlayerId => _roomEntity.PlayerEntityId;

    public string PlayerName { get; set; }

    public RoomStateType State
    {
        get => _roomEntity.RoomState;
        set
        {
            _roomEntity.RoomState = value;
            _storageQueue.Add(_roomEntity);
        }
    }

    public string Password
    {
        get => _roomEntity.Password;
        set
        {
            _roomEntity.Password = value;
            _storageQueue.Add(_roomEntity);
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
            _storageQueue.Add(_roomEntity);
        }
    }

    public int UsersMax
    {
        get => _roomEntity.UsersMax;
        set
        {
            _roomEntity.UsersMax = value;
            _storageQueue.Add(_roomEntity);
        }
    }

    public double PaintWall
    {
        get => _roomEntity.PaintWall;
        set
        {
            _roomEntity.PaintWall = value;
            _storageQueue.Add(_roomEntity);
        }
    }

    public double PaintFloor
    {
        get => _roomEntity.PaintFloor;
        set
        {
            _roomEntity.PaintFloor = value;
            _storageQueue.Add(_roomEntity);
        }
    }

    public double PaintLandscape
    {
        get => _roomEntity.PaintLandscape;
        set
        {
            _roomEntity.PaintLandscape = value;
            _storageQueue.Add(_roomEntity);
        }
    }

    public int WallHeight
    {
        get => _roomEntity.WallHeight;
        set
        {
            _roomEntity.WallHeight = value;
            _storageQueue.Add(_roomEntity);
        }
    }

    public bool HideWalls
    {
        get => (bool)_roomEntity.HideWalls;
        set
        {
            _roomEntity.HideWalls = value;
            _storageQueue.Add(_roomEntity);
        }
    }

    public RoomThicknessType ThicknessWall
    {
        get => _roomEntity.ThicknessWall;
        set
        {
            _roomEntity.ThicknessWall = value;
            _storageQueue.Add(_roomEntity);
        }
    }

    public RoomThicknessType ThicknessFloor
    {
        get => _roomEntity.ThicknessFloor;
        set
        {
            _roomEntity.ThicknessFloor = value;
            _storageQueue.Add(_roomEntity);
        }
    }

    public bool BlockingDisabled
    {
        get => (bool)_roomEntity.AllowWalkThrough;
        set
        {
            _roomEntity.AllowWalkThrough = value;
            _storageQueue.Add(_roomEntity);
        }
    }

    public bool AllowEditing
    {
        get => (bool)_roomEntity.AllowEditing;
        set
        {
            _roomEntity.AllowEditing = value;
            _storageQueue.Add(_roomEntity);
        }
    }

    public bool AllowPets
    {
        get => (bool)_roomEntity.AllowPets;
        set
        {
            _roomEntity.AllowPets = value;
            _storageQueue.Add(_roomEntity);
        }
    }

    public bool AllowPetsEat
    {
        get => (bool)_roomEntity.AllowPetsEat;
        set
        {
            _roomEntity.AllowPetsEat = value;
            _storageQueue.Add(_roomEntity);
        }
    }

    public RoomTradeType TradeType
    {
        get => _roomEntity.TradeType;
        set
        {
            _roomEntity.TradeType = value;
            _storageQueue.Add(_roomEntity);
        }
    }

    public RoomMuteType MuteType
    {
        get => _roomEntity.MuteType;
        set
        {
            _roomEntity.MuteType = value;
            _storageQueue.Add(_roomEntity);
        }
    }

    public RoomKickType KickType
    {
        get => _roomEntity.KickType;
        set
        {
            _roomEntity.KickType = value;
            _storageQueue.Add(_roomEntity);
        }
    }

    public RoomBanType BanType
    {
        get => _roomEntity.BanType;
        set
        {
            _roomEntity.BanType = value;
            _storageQueue.Add(_roomEntity);
        }
    }

    public RoomChatModeType ChatModeType
    {
        get => _roomEntity.ChatModeType;
        set
        {
            _roomEntity.ChatModeType = value;
            _storageQueue.Add(_roomEntity);
        }
    }

    public RoomChatWeightType ChatWeightType
    {
        get => _roomEntity.ChatWeightType;
        set
        {
            _roomEntity.ChatWeightType = value;
            _storageQueue.Add(_roomEntity);
        }
    }

    public RoomChatSpeedType ChatSpeedType
    {
        get => _roomEntity.ChatSpeedType;
        set
        {
            _roomEntity.ChatSpeedType = value;
            _storageQueue.Add(_roomEntity);
        }
    }

    public RoomChatProtectionType ChatProtectionType
    {
        get => _roomEntity.ChatProtectionType;
        set
        {
            _roomEntity.ChatProtectionType = value;
            _storageQueue.Add(_roomEntity);
        }
    }

    public int ChatDistance
    {
        get => _roomEntity.ChatDistance;
        set
        {
            _roomEntity.ChatDistance = value;
            _storageQueue.Add(_roomEntity);
        }
    }

    public DateTime LastActive
    {
        get => _roomEntity.LastActive;
        set
        {
            _roomEntity.LastActive = value;
            _storageQueue.Add(_roomEntity);
        }
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

    public void UpdateRoomVisualization(bool hideWalls, RoomThicknessType wallThickness,
        RoomThicknessType floorThickness)
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
    }

    public void UpdateRoomChat(RoomChatModeType modeType, RoomChatWeightType weightType, RoomChatSpeedType speedType,
        int distance, RoomChatProtectionType protectionType)
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
    }
}