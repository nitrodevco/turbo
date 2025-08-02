using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;

namespace Turbo.Rooms.Factories;

public interface IRoomChatFactory
{
    public IRoomChatManager Create(IRoom room);
}