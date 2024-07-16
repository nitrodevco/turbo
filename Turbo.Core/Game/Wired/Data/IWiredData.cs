using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Packets.Messages;

namespace Turbo.Core.Game.Wired.Data
{
    public interface IWiredData
    {
        public bool SetFromMessage(IMessageEvent update);
        public bool SetRoomObject(IRoomObjectFloor roomObject);
        public int Id { get; }
        public int SpriteId { get; }
        public int WiredType { get; }
        public bool SelectionEnabled { get; }
        public int SelectionLimit { get; }
        public IList<int> SelectionIds { get; set; }
        public string StringParameter { get; set; }
        public IList<int> IntParameters { get; set; }
    }
}
