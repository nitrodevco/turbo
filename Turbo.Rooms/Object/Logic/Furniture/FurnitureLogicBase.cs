using Turbo.Core.Game.Furniture.Constants;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Data;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Packets.Outgoing.Room.Engine;
using Turbo.Rooms.Object.Data;
using Turbo.Rooms.Object.Data.Types;

namespace Turbo.Rooms.Object.Logic.Furniture
{
    public class FurnitureLogicBase : RollingObjectLogic, IFurnitureLogic
    {
        public IFurnitureDefinition FurnitureDefinition { get; private set; }

        public IStuffData StuffData { get; protected set; }

        public override void Dispose()
        {
            base.Dispose();
        }

        public virtual bool Setup(IFurnitureDefinition furnitureDefinition, string jsonString = null)
        {
            if (furnitureDefinition == null) return false;

            FurnitureDefinition = furnitureDefinition;

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

        public void RefreshFurniture()
        {
            RoomObject.Room.SendComposer(new ObjectUpdateMessage
            {
                Object = RoomObject
            });
        }

        public void RefreshStuffData()
        {
            RoomObject.Room.SendComposer(new ObjectDataUpdateMessage
            {
                Object = RoomObject
            });
        }

        public virtual bool SetState(int state, bool refresh = true)
        {
            return false;
        }

        public virtual void OnEnter(IRoomObject roomObject)
        {
            return;
        }

        public virtual void OnLeave(IRoomObject roomObject)
        {
            return;
        }

        public virtual void BeforeStep(IRoomObject roomObject)
        {
            return;
        }

        public virtual void OnStep(IRoomObject roomObject)
        {
            return;
        }

        public virtual void OnStop(IRoomObject roomObject)
        {
            return;
        }

        public virtual void OnInteract(IRoomObject roomObject, int param)
        {
            return;
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

        public virtual bool CanStack() => FurnitureDefinition.CanStack;

        public virtual bool CanWalk(IRoomObject roomObject = null) => FurnitureDefinition.CanWalk;

        public virtual bool CanSit(IRoomObject roomObject = null) => FurnitureDefinition.CanSit;

        public virtual bool CanLay(IRoomObject roomObject = null) => FurnitureDefinition.CanLay;

        public virtual bool CanRoll() => true;

        public virtual bool CanToggle(IRoomObject roomObject)
        {
            if (UsagePolicy == FurniUsagePolicy.Nobody) return false;

            if(UsagePolicy == FurniUsagePolicy.Controller)
            {
                if(roomObject.RoomObjectHolder is IRoomManipulator roomManipulator)
                {
                    if (RoomObject.Room.RoomSecurityManager.IsController(roomManipulator)) return true;
                }

                return false;
            }

            return true;
        }

        public virtual bool IsOpen(IRoomObject roomObject = null) => CanWalk(roomObject) || CanSit(roomObject) || CanLay(roomObject);

        public virtual FurniUsagePolicy UsagePolicy => (FurnitureDefinition.TotalStates == 0) ? FurniUsagePolicy.Nobody : FurnitureDefinition.UsagePolicy;

        public virtual double StackHeight => FurnitureDefinition.Z;

        public StuffDataKey DataKey => StuffDataKey.LegacyKey;

        public double Height => RoomObject.Location.Z + StackHeight;
    }
}
