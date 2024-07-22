using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Core.Game.Wired.Data;

namespace Turbo.Core.Game.Wired;

public interface IFurnitureWiredLogic : IFurnitureFloorLogic
{
    public IWiredData WiredData { get; }
    public int WiredKey { get; }
    public void SetupWiredData(string jsonString = null);
    public IWiredData CreateWiredDataFromJson(string jsonString = null);
    public bool SaveWiredData(IRoomObjectAvatar avatar, IWiredData wiredData);

    public bool CanTrigger(IWiredArguments wiredArguments = null);
    public void OnTriggered(IWiredArguments wiredArguments = null);
}