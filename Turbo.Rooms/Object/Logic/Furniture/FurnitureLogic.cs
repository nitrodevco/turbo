using Turbo.Core.Game.Furniture.Definition;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Data;
using Turbo.Rooms.Object.Logic.Avatar;

namespace Turbo.Rooms.Object.Logic.Furniture
{
    public class FurnitureLogic : FurnitureLogicBase
    {
        public override bool Setup(IFurnitureDefinition furnitureDefinition, string jsonString = null)
        {
            if (!base.Setup(furnitureDefinition)) return false;

            IStuffData stuffData = CreateStuffDataFromJson(jsonString);

            _stuffData = stuffData;

            return true;
        }

        public override void OnStop(IRoomObject roomObject)
        {
            if(roomObject.Logic is AvatarLogic avatarLogic)
            {
                if(CanSit())
                {
                    avatarLogic.Sit(true, StackHeight(), RoomObject.Location.Rotation);

                    return;
                }

                if (CanLay())
                {
                    avatarLogic.Lay(true, StackHeight(), RoomObject.Location.Rotation);

                    return;
                }
            }
        }

        protected virtual int GetNextToggleableState()
        {
            int totalStates = FurnitureDefinition.TotalStates;

            if (totalStates == 0) return 0;

            return (_stuffData.GetState() + 1) % totalStates;
        }
    }
}
