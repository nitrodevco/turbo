using System.Threading.Tasks;
using Turbo.Core.Game.Furniture.Constants;
using Turbo.Core.Game.Furniture.Data;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Furniture.Data;

namespace Turbo.Rooms.Object.Logic.Furniture;

public abstract class FurnitureLogicBase : RoomObjectLogicBase, IFurnitureLogic
{
    public IFurnitureDefinition FurnitureDefinition { get; private set; }

    public IStuffData StuffData { get; protected set; }

    public virtual async Task<bool> Setup(IFurnitureDefinition furnitureDefinition, string jsonString = null)
    {
        if (furnitureDefinition == null) return false;

        FurnitureDefinition = furnitureDefinition;

        var stuffData = CreateStuffDataFromJson(jsonString);

        StuffData = stuffData;

        return true;
    }

    public abstract void RefreshFurniture();

    public abstract void RefreshStuffData();

    public virtual bool SetState(int state, bool refresh = true)
    {
        return false;
    }

    public virtual void OnInteract(IRoomObjectAvatar avatar, int param)
    {
        if (!CanToggle(avatar)) return;

        param = GetNextToggleableState();

        if (!SetState(param)) return;
    }

    public virtual void OnPlace(IRoomManipulator roomManipulator)
    {
    }

    public virtual void OnMove(IRoomManipulator roomManipulator)
    {
    }

    public virtual void OnPickup(IRoomManipulator roomManipulator)
    {
    }

    public virtual bool CanToggle(IRoomObjectAvatar avatar)
    {
        return false;
    }

    public virtual FurniUsagePolicy UsagePolicy => FurnitureDefinition.TotalStates == 0
        ? FurniUsagePolicy.Nobody
        : FurnitureDefinition.UsagePolicy;

    public virtual StuffDataKey DataKey => StuffDataKey.LegacyKey;

    protected IStuffData CreateStuffData()
    {
        return StuffDataFactory.CreateStuffData((int)DataKey);
    }

    protected IStuffData CreateStuffDataFromJson(string jsonString)
    {
        return StuffDataFactory.CreateStuffDataFromJson((int)DataKey, jsonString);
    }

    protected virtual int GetNextToggleableState()
    {
        var totalStates = FurnitureDefinition.TotalStates;

        if (totalStates == 0) return 0;

        return (StuffData.GetState() + 1) % totalStates;
    }
}