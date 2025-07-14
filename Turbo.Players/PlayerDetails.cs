using System;
using System.Threading.Tasks;
using Turbo.Core.Database.Entities.Players;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Players.Constants;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Storage;

namespace Turbo.Players;

public class PlayerDetails(
    IPlayerManager _playerManager,
    PlayerEntity _playerEntity,
    IStorageQueue _storageQueue) : IPlayerDetails
{
    private int _cachedChatStyleId = -1;

    public int? ChatStyleId
    {
        get => _playerEntity.RoomChatStyleId == null ? -1 : _playerEntity.RoomChatStyleId;
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
        get => _playerEntity.Motto == null ? "" : _playerEntity.Motto;
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

    public DateTime CreatedAt => _playerEntity.CreatedAt;

    public DateTime UpdatedAt => _playerEntity.UpdatedAt;

    public DateTime? DeletedAt => _playerEntity.DeletedAt;
}