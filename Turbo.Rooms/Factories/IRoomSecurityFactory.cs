using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;

namespace Turbo.Core.Database.Factories.Rooms;

public interface IRoomSecurityFactory
{
    public IRoomSecurityManager Create(IRoom room);
}