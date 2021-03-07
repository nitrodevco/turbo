using System.Threading.Tasks;

namespace Turbo.Rooms
{
    public interface IRoomContainer
    {
        public ValueTask RemoveRoom(int id);
    }
}
