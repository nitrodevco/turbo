using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Players.Constants;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Storage;
using Turbo.Database.Entities.Players;
using Turbo.Players.Constants;

namespace Turbo.Players;

public class PlayerDetails(
    PlayerEntity _playerEntity,
    IStorageQueue _storageQueue) : IPlayerDetails
{
    private readonly int _cachedChatStyleId = PlayerConstants.InvalidChatStyleId;

    public int? ChatStyleId
    {
        get => _playerEntity.RoomChatStyleId ?? PlayerConstants.InvalidChatStyleId;
        set
        {
            _playerEntity.RoomChatStyleId = value;
            _storageQueue.Add(_playerEntity);
        }
    }

    public async Task DisposeAsync()
    {
        await _storageQueue.SaveEntityNow(_playerEntity);
    }

    public int GetValidChatStyleId(int styleId)
    {
        return styleId;
    }

    public void SetPreferredChatStyleByClientId(int styleId)
    {
        ChatStyleId = GetValidChatStyleId(styleId);
    }

    public int Id => _playerEntity.Id;

    public string Name
    {
        get => _playerEntity.Name;
        set
        {
            _playerEntity.Name = value;
            _storageQueue.Add(_playerEntity);

        }
    }

    public string Motto
    {
        get => _playerEntity.Motto ?? string.Empty;
        set
        {
            _playerEntity.Motto = value;
            _storageQueue.Add(_playerEntity);
        }
    }

    public string Figure
    {
        get => _playerEntity.Figure;
        set
        {
            _playerEntity.Figure = value;
            _storageQueue.Add(_playerEntity);
        }
    }

    public AvatarGender Gender
    {
        get => _playerEntity.Gender;
        set
        {
            _playerEntity.Gender = value;
            _storageQueue.Add(_playerEntity);
        }
    }

    public PlayerStatusEnum PlayerStatus
    {
        get => _playerEntity.PlayerStatus;
        set
        {
            _playerEntity.PlayerStatus = value;
            _storageQueue.Add(_playerEntity);
        }
    }

    public IList<PlayerPerkEnum> PlayerPerks
    {
        get
        {
            var perkFlags = _playerEntity.PlayerPerks;
            var result = new List<PlayerPerkEnum>();

            foreach (PlayerPerkEnum flag in Enum.GetValues(typeof(PlayerPerkEnum)))
            {
                if (Convert.ToInt32(flag) != 0 && ((PlayerPerkEnum)perkFlags).HasFlag(flag))
                {
                    result.Add(flag);
                }
            }
            return result;
        }
        set
        {
            var perkFlags = 0;

            foreach (var flag in value)
            {
                perkFlags |= Convert.ToInt32(flag); // bitwise OR
            }

            _playerEntity.PlayerPerks = perkFlags;
            _storageQueue.Add(_playerEntity);
        }
    }

    public DateTime CreatedAt => _playerEntity.CreatedAt;

    public DateTime UpdatedAt => _playerEntity.UpdatedAt;

    public DateTime? DeletedAt => _playerEntity.DeletedAt;
}