using System.Text.Json;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Object.Logic.Wired;
using Turbo.Core.Game.Rooms.Object.Logic.Wired.Data;
using Turbo.Rooms.Object.Logic.Furniture.Wired.Data;

namespace Turbo.Rooms.Object.Logic.Furniture.Wired
{
    public class FurnitureWiredLogic : FurnitureLogic, IFurnitureWiredLogic
    {
        protected IWiredData _wiredData;

        public void SetupWiredData(string jsonString = null)
        {
            IWiredData wiredData = CreateWiredDataFromJson(jsonString);

            _wiredData = wiredData;
        }

        public virtual IWiredData CreateWiredDataFromJson(string jsonString = null)
        {
            return JsonSerializer.Deserialize<WiredDataBase>(jsonString);
        }

        public virtual IWiredData WiredData => _wiredData;
        public int WiredKey => 0;

        public virtual bool RequiresPlayer => false;
    }
}
