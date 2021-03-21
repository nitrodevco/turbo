using Turbo.Core.Game.Furniture.Definition;

namespace Turbo.Core.Game.Rooms.Object.Logic
{
    public interface IFurnitureLogic
    {
        public void Setup(IFurnitureDefinition furnitureDefinition);

        public void OnEnter(IRoomObject roomObject);
        public void OnLeave(IRoomObject roomObject);
        public void BeforeStep(IRoomObject roomObject);
        public void OnStep(IRoomObject roomObject);
        public void OnStop(IRoomObject roomObject);
        public void OnInteract(IRoomObject roomObject);
        public void OnPlace(IRoomManipulator roomManipulator);
        public void OnMove(IRoomManipulator roomManipulator);
        public void OnPickup(IRoomManipulator roomManipulator);
        public bool CanStack();
        public bool CanWalk();
        public bool CanSit();
        public bool CanLay();
        public bool CanRoll();
        public bool CanToggle();
        public bool IsOpen();
        public double StackHeight();
    }
}
