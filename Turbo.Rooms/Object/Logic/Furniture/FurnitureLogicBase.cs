using System.Threading.Tasks;
using Turbo.Core.Game.Furniture.Constants;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Furniture.Data;
using Turbo.Core.Game.Furniture.Data;
using Turbo.Core.Game.Rooms.Constants;

namespace Turbo.Rooms.Object.Logic.Furniture
{
    public abstract class FurnitureLogicBase : RoomObjectLogicBase, IFurnitureLogic
    {
        public IFurnitureDefinition FurnitureDefinition { get; private set; }

        public IStuffData StuffData { get; protected set; }

        public virtual async Task<bool> Setup(IFurnitureDefinition furnitureDefinition, string jsonString = null)
        {
            if (furnitureDefinition == null) return false;

            FurnitureDefinition = furnitureDefinition;

            IStuffData stuffData = CreateStuffDataFromJson(jsonString);

            StuffData = stuffData;

            return true;
        }

        protected IStuffData CreateStuffData()
        {
            return StuffDataFactory.CreateStuffData((int)DataKey);
        }

        protected IStuffData CreateStuffDataFromJson(string jsonString)
        {
            return StuffDataFactory.CreateStuffDataFromJson((int)DataKey, jsonString);
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
            return;
        }

        public virtual void OnMove(IRoomManipulator roomManipulator)
        {
            return;
        }

        public virtual void OnPickup(IRoomManipulator roomManipulator)
        {
            return;
        }

        public virtual bool CanToggle(IRoomObjectAvatar avatar)
        {
            return false;
        }

        protected virtual int GetNextToggleableState()
        {
            int totalStates = FurnitureDefinition.TotalStates;

            if (totalStates == 0) return 0;

            return (StuffData.GetState() + 1) % totalStates;
        }

        public virtual FurniUsagePolicy UsagePolicy => (FurnitureDefinition.TotalStates == 0) ? FurniUsagePolicy.Nobody : FurnitureDefinition.UsagePolicy;

        public virtual StuffDataKey DataKey => StuffDataKey.LegacyKey;
    }
}
