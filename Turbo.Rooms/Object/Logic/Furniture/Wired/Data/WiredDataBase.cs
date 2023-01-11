using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Core.Game.Rooms.Object.Logic.Wired;
using Turbo.Core.Game.Rooms.Object.Logic.Wired.Data;

namespace Turbo.Rooms.Object.Logic.Furniture.Wired.Data
{
    public class WiredDataBase : IWiredData
    {
        private static readonly int _selectionLimit = 5;

        [JsonIgnore]
        public int Id { get; protected set; }

        [JsonIgnore]
        public int SpriteId { get; protected set; }

        [JsonIgnore]
        public int WiredType { get; protected set; }

        [JsonIgnore]
        public bool SelectionEnabled { get; protected set; }

        [JsonIgnore]
        public int SelectionLimit { get; protected set; }

        public IList<int> SelectionIds { get; set; }
        public string StringParameter { get; set; }
        public IList<int> IntParameters { get; set; }

        public WiredDataBase()
        {
            SelectionLimit = _selectionLimit;
            SelectionIds = new List<int>();
            StringParameter = "";
            IntParameters = new List<int>();
        }

        public virtual bool SetRoomObject(IRoomObjectFloor roomObject)
        {
            if (roomObject.Logic is not IFurnitureWiredLogic wiredLogic) return false;

            Id = roomObject.Id;
            SpriteId = wiredLogic.FurnitureDefinition.SpriteId;
            WiredType = wiredLogic.WiredKey;

            return true;
        }
    }
}
