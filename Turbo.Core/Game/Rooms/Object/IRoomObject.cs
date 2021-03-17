using System;
using Turbo.Core.Game.Rooms.Messages;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Core.Game.Rooms.Object
{
    public interface IRoomObject : IDisposable
    {
        public IRoom Room { get; }
        public IRoomObjectHolder RoomObjectHolder { get; }

        public int Id { get; }
        public string Type { get; }

        public IPoint Location { get; }

        public IRoomObjectLogic Logic { get; }

        public bool NeedsUpdate { get; set; }

        public void SetLocation(IPoint point);
        public bool SetHolder(IRoomObjectHolder roomObjectHolder);
        public void SetLogic(IRoomObjectLogic logic);
    }
}
