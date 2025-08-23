using System;
using Turbo.Core.Game.Players.Constants;
using Turbo.Core.Game.Rooms.Object.Constants;

namespace Turbo.Core.Game.Messenger;
public interface IMessengerSearchResult
{
    int Id { get; }
    string Name { get; }
    string Motto { get; }
    string Figure { get; }
    PlayerStatusEnum Status { get; }
    AvatarGender Gender { get; }
    DateTime LastOnline { get; }
    bool CanFollow { get; }
    string RealName { get; }
}
