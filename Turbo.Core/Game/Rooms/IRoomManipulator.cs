using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Security;

namespace Turbo.Core.Game.Rooms;

public interface IRoomManipulator : IPermissionHolder, ISessionHolder
{
    public int Id { get; }
}