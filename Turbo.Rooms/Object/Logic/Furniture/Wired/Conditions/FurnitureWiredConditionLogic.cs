using System.Text.Json;
using Turbo.Core.Game.Rooms.Object.Logic.Wired.Data;
using Turbo.Rooms.Object.Logic.Furniture.Wired.Data.Types;

namespace Turbo.Rooms.Object.Logic.Furniture.Wired.Conditions
{
    public class FurnitureWiredConditionLogic : FurnitureWiredLogic
    {
        public override IWiredData CreateWiredDataFromJson(string jsonString = null)
        {
            if (jsonString == null) return new WiredConditionData();

            return JsonSerializer.Deserialize<WiredConditionData>(jsonString);
        }

        public override WiredConditionData WiredData => (WiredConditionData)_wiredData;
    }
}
