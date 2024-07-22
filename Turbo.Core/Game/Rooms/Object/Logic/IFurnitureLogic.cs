using System.Threading.Tasks;
using Turbo.Core.Game.Furniture.Constants;
using Turbo.Core.Game.Furniture.Data;
using Turbo.Core.Game.Furniture.Definition;

namespace Turbo.Core.Game.Rooms.Object.Logic;

public interface IFurnitureLogic : IRoomObjectLogic
{
    public IFurnitureDefinition FurnitureDefinition { get; }
    public IStuffData StuffData { get; }
    public FurniUsagePolicy UsagePolicy { get; }
    public StuffDataKey DataKey { get; }
    public Task<bool> Setup(IFurnitureDefinition furnitureDefinition, string jsonString = null);
    public void RefreshFurniture();
    public void RefreshStuffData();
    public bool SetState(int state, bool refresh = true);
    public void OnInteract(IRoomObjectAvatar avatar, int param);
    public void OnPlace(IRoomManipulator roomManipulator);
    public void OnMove(IRoomManipulator roomManipulator);
    public void OnPickup(IRoomManipulator roomManipulator);
    public bool CanToggle(IRoomObjectAvatar avatar);
}