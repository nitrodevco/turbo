using System.Threading.Tasks;

namespace Turbo.Core.Game.Rooms
{
    public interface IRoomContainer
    {
        public ValueTask RemoveRoom(int id);
    }
}
