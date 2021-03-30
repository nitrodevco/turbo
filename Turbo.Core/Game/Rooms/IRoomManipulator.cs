using Turbo.Core.Networking.Game.Clients;

namespace Turbo.Core.Game.Rooms
{
    public interface IRoomManipulator
    {
        public ISession Session { get; }
        public bool HasPermission(string permission);
        public int Id { get; }
    }
}
