using Turbo.Core.Game.Furniture.Definition;
using Turbo.Core.Game.Rooms.Object.Data;

namespace Turbo.Core.Game.Rooms.Object.Logic
{
    public interface IFurnitureLogic : IRollingObjectLogic
    {
        public IFurnitureDefinition FurnitureDefinition { get; }
        public IStuffData StuffData { get; }
        public bool Setup(IFurnitureDefinition furnitureDefinition, string jsonString = null);
        public void RefreshFurniture();
        public void RefreshStuffData();
        public bool SetState(int state, bool refresh = true);
        public void OnEnter(IRoomObject roomObject);
        public void OnLeave(IRoomObject roomObject);
        public void BeforeStep(IRoomObject roomObject);
        public void OnStep(IRoomObject roomObject);
        public void OnStop(IRoomObject roomObject);
        public void OnInteract(IRoomObject roomObject, int param);
        public void OnPlace(IRoomManipulator roomManipulator);
        public void OnMove(IRoomManipulator roomManipulator);
        public void OnPickup(IRoomManipulator roomManipulator);
        public bool CanStack();
        public bool CanWalk();
        public bool CanSit();
        public bool CanLay();
        public bool CanRoll();
        public bool CanToggle(IRoomObject roomObject);
        public bool IsOpen();
        public double StackHeight();
        public StuffDataKey DataKey { get; }
        public double Height { get; }
    }
}
