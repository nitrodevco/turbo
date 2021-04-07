using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Object.Logic.Wired.Data;
using Turbo.Rooms.Object.Logic.Furniture.Wired.Data.Types;

namespace Turbo.Rooms.Object.Logic.Furniture.Wired.Triggers
{
    public class FurnitureWiredTriggerLogic : FurnitureWiredLogic
    {
        public override IWiredData CreateWiredDataFromJson(string jsonString = null)
        {
            if (jsonString == null) return new WiredTriggerData();

            return JsonSerializer.Deserialize<WiredTriggerData>(jsonString);
        }

        public override WiredTriggerData WiredData => (WiredTriggerData)_wiredData;
    }
}
