using Turbo.Core.Game.Messenger.Constants;
using Turbo.Core.Game.Players.Constants;
using Turbo.Core.Game.Rooms.Object.Constants;

namespace Turbo.Core.Game.Messenger;

public interface IMessengerFriend
{
    int Id { get; }
    string Name { get; set; }
    AvatarGender Gender { get; set; }
    PlayerStatusEnum Status { get; set; }
    string Figure { get; set; }
    string Motto { get; set; }
    int CategoryId { get; set; }
    MessengerFriendRelationEnum Relation { get; set; }
    bool InRoom { get; set; }
}
