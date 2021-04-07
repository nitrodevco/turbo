using System.Text.Json;
using Turbo.Core.Game.Rooms.Object.Logic.Wired.Data;
using Turbo.Rooms.Object.Logic.Furniture.Wired.Data.Types;

namespace Turbo.Rooms.Object.Logic.Furniture.Wired.Actions
{
    public class FurnitureWiredActionLogic : FurnitureWiredLogic
    {
        public override IWiredData CreateWiredDataFromJson(string jsonString = null)
        {
            if (jsonString == null) return new WiredActionData();

            return JsonSerializer.Deserialize<WiredActionData>(jsonString);
        }

        public override WiredActionData WiredData => (WiredActionData)_wiredData;
    }
}
