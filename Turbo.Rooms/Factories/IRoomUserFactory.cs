using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;

namespace Turbo.Rooms.Factories;

public interface IRoomUserFactory
{
    public IRoomUserManager Create(IRoom room);
}