using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Data;
using Turbo.Rooms.Object.Logic.Avatar;

namespace Turbo.Rooms.Object.Logic.Furniture
{
    public class FurnitureLogic : FurnitureLogicBase
    {
        public override async Task<bool> Setup(IFurnitureDefinition furnitureDefinition, string jsonString = null)
        {
            if (!await base.Setup(furnitureDefinition, jsonString)) return false;

            IStuffData stuffData = CreateStuffDataFromJson(jsonString);

            StuffData = stuffData;

            return true;
        }

        public override bool SetState(int state, bool refresh = true)
        {
            if (StuffData == null) return false;

            if (state == StuffData.GetState()) return false;

            StuffData.SetState(state.ToString());

            if (RoomObject.RoomObjectHolder is IFurniture furniture)
            {
                furniture.Save();
            }

            if (refresh) RefreshStuffData();

            return true;
        }

        public override void OnStop(IRoomObject roomObject)
        {
            if (roomObject.Logic is AvatarLogic avatarLogic)
            {
                if (CanSit())
                {
                    avatarLogic.Sit(true, StackHeight, RoomObject.Location.Rotation);

                    return;
                }

                if (CanLay())
                {
                    avatarLogic.Lay(true, StackHeight, RoomObject.Location.Rotation);

                    return;
                }
            }
        }

        public override void OnInteract(IRoomObject roomObject, int param = 0)
        {
            if (!CanToggle(roomObject)) return;

            param = GetNextToggleableState();

            if (!SetState(param)) return;

            base.OnInteract(roomObject, param);
        }

        protected virtual int GetNextToggleableState()
        {
            int totalStates = FurnitureDefinition.TotalStates;

            if (totalStates == 0) return 0;

            return (StuffData.GetState() + 1) % totalStates;
        }
    }
}
