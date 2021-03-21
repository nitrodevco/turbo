using Turbo.Core.Game.Rooms;
using Turbo.Database.Entities.Room;

namespace Turbo.Rooms
{
    public class RoomDetails : IRoomDetails
    {
        private readonly RoomEntity _roomEntity;

        public RoomDetails(RoomEntity roomEntity)
        {
            _roomEntity = roomEntity;
        }

        public int Id => _roomEntity.Id;

        public string Name
        {
            get => _roomEntity.Name;
        }

        public int ModelId => _roomEntity.RoomModelEntityId;

        public bool AllowWalkThrough
        {
            get => _roomEntity.AllowWalkThrough;
        }
    }
}
