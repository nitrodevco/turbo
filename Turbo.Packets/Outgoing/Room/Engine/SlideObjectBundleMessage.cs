using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Engine
{
    public record SlideObjectBundleMessage : IComposer
    {
        public IPoint OldPos { get; init; }
        public IPoint NewPos { get; init; }
        public List<IRoomObject> Objects { get; init; }
        public int RollerItemId { get; init; }

        /// <summary>
        /// Optional, set as null or 0 if no Avatar
        /// </summary>
        public int? AvatarId { get; init; }

        /// <summary>
        /// Options are {1 = MOVE, 2 = SLIDE}
        /// </summary>
        public int? MoveType { get; init; }
    }
}
