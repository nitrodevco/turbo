namespace Turbo.Rooms.Object.Logic.Furniture
{
    public class FurnitureStackHelperLogic : FurnitureLogic
    {
        public override bool CanStack() => false;

        public override bool CanRoll() => false;

        public override bool CanToggle() => false;

        public override bool IsOpen() => false;
    }
}
