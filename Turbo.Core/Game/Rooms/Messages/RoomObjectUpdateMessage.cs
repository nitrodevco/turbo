using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Core.Game.Rooms.Messages
{
    public class RoomObjectUpdateMessage
    {
        public IPoint Location { get; private set; }

        public RoomObjectUpdateMessage(IPoint location)
        {
            Location = location;
        }
    }
}
