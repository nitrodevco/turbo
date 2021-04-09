using System.Threading.Tasks;

namespace Turbo.Core.Game.Rooms.Object
{
    public interface IRoomObjectHolder
    {
        public IRoomObject RoomObject { get; }

        public bool SetRoomObject(IRoomObject roomObject);
        public void ClearRoomObject();

        public int Id { get; }
        public string Type { get; }
    }
}
