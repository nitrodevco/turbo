using Turbo.Core.Game.Messenger;
using Turbo.Core.Game.Messenger.Constants;
using Turbo.Core.Game.Players.Constants;
using Turbo.Core.Game.Rooms.Object.Constants; // Added back for AvatarGender
using Turbo.Database.Entities.Messenger;

namespace Turbo.Messenger;

public class MessengerFriend(MessengerFriendEntity entity) : IMessengerFriend
{
    private bool _inRoom;

    public int Id => entity.FriendPlayerEntityId;

    public string Name
    {
        get => entity.FriendPlayerEntity.Name;
        set
        {
            if (value == entity.FriendPlayerEntity.Name) return;
            entity.FriendPlayerEntity.Name = value;
        }
    }

    public AvatarGender Gender
    {
        get => entity.FriendPlayerEntity.Gender;
        set
        {
            if (value == entity.FriendPlayerEntity.Gender) return;
            entity.FriendPlayerEntity.Gender = value;
        }
    }

    public PlayerStatusEnum Status
    {
        get => entity.FriendPlayerEntity.PlayerStatus;
        set
        {
            if (value == entity.FriendPlayerEntity.PlayerStatus) return;
            entity.FriendPlayerEntity.PlayerStatus = value;
        }
    }

    public string Figure
    {
        get => entity.FriendPlayerEntity.Figure;
        set
        {
            if (value == entity.FriendPlayerEntity.Figure) return;
            entity.FriendPlayerEntity.Figure = value;
        }
    }

    public string Motto
    {
        get => entity.FriendPlayerEntity.Motto ?? string.Empty;
        set
        {
            if (value == entity.FriendPlayerEntity.Motto) return;
            entity.FriendPlayerEntity.Motto = value;
        }
    }

    public int CategoryId
    {
        get => entity.MessengerCategoryEntityId ?? -1;
        set
        {
            int? mapped = value <= 0 ? null : value;
            if (mapped == entity.MessengerCategoryEntityId) return;
            entity.MessengerCategoryEntityId = mapped;
        }
    }

    public MessengerFriendRelationEnum Relation
    {
        get => entity.RelationType;
        set
        {
            if (value == entity.RelationType) return;
            entity.RelationType = value;
        }
    }

    public bool InRoom
    {
        get => _inRoom;
        set => _inRoom = value;
    }
}
