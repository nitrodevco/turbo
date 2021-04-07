using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Core.Game.Rooms.Object.Logic.Wired.Data
{
    public interface IWiredData
    {
        public int Id { get; }
        public int SpriteId { get; }
        public int WiredType { get; }
        public bool SelectionEnabled { get; }
        public int SelectionLimit { get; }
    }
}
