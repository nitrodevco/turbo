using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;

namespace Turbo.Rooms.Factories;

public interface IRoomSecurityFactory
{
    public IRoomSecurityManager Create(IRoom room);
}