using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Core.Game.Wired.Data;

namespace Turbo.Core.Game.Wired;

public interface IFurnitureWiredLogic : IFurnitureFloorLogic
{
    public IWiredData WiredData { get; }
    public int WiredKey { get; }
    public void SetupWiredData(string jsonString);
    public IWiredData CreateWiredDataFromJson(string jsonString);
    public bool SaveWiredData(IRoomObjectAvatar avatar, IWiredData wiredData);

    public bool CanTrigger(IWiredArguments wiredArguments);
    public void OnTriggered(IWiredArguments wiredArguments);
}