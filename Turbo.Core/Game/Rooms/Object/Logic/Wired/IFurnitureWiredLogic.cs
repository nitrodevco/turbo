using Turbo.Core.Game.Rooms.Object.Logic.Wired.Data;

namespace Turbo.Core.Game.Rooms.Object.Logic.Wired
{
    public interface IFurnitureWiredLogic : IFurnitureLogic
    {
        public void SetupWiredData(string jsonString = null);
        public IWiredData CreateWiredDataFromJson(string jsonString = null);

        public bool CanTrigger(IWiredArguments wiredArguments = null);
        public void OnTriggered(IWiredArguments wiredArguments = null);

        public IWiredData WiredData { get; }
        public int WiredKey { get; }
    }
}
