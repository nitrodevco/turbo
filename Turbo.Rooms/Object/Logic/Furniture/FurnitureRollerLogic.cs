namespace Turbo.Rooms.Object.Logic.Furniture
{
    public class FurnitureRollerLogic : FurnitureLogic
    {
        public override bool CanStack() => false;

        public override bool CanRoll() => false;

        public override bool CanToggle() => false;
    }
}
