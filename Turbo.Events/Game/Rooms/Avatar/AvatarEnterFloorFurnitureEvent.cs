using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Events.Game.Rooms.Avatar
{
    public class AvatarEnterFloorFurnitureEvent : TurboEvent
    {
        public IRoomObjectAvatar AvatarObject { get; init; }
        public IRoomObjectFloor FloorObject { get; init; }
    }
}
