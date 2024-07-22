using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Furniture.Constants;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Constants;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Packets.Outgoing.Room.Engine;
using Turbo.Rooms.Object.Attributes;

namespace Turbo.Rooms.Object.Logic.Furniture;

[RoomObjectLogic("default_wall")]
public class FurnitureWallLogic : FurnitureLogicBase, IFurnitureWallLogic
{
    public IRoomObjectWall RoomObject { get; private set; }

    public bool SetRoomObject(IRoomObjectWall roomObject)
    {
        if (roomObject == RoomObject) return true;

        if (RoomObject != null) RoomObject.SetLogic(null);

        if (roomObject == null)
        {
            Dispose();

            RoomObject = null;

            return false;
        }

        RoomObject = roomObject;

        RoomObject.SetLogic(this);

        return true;
    }

    public override void RefreshFurniture()
    {
        RoomObject.Room.SendComposer(new ItemUpdateMessage
        {
            Object = RoomObject
        });
    }

    public override void RefreshStuffData()
    {
        RefreshFurniture();

        /* RoomObject.Room.SendComposer(new ItemDataUpdateMessage
        {
            ItemId = RoomObject.Id,
            ItemData = StuffData.GetLegacyString()
        }); */
    }

    public override bool SetState(int state, bool refresh = true)
    {
        if (StuffData == null) return false;

        if (state == StuffData.GetState()) return false;

        StuffData.SetState(state.ToString());

        if (RoomObject.RoomObjectHolder is IRoomWallFurniture wallFurniture) wallFurniture.Save();

        if (refresh) RefreshStuffData();

        return true;
    }

    public override bool CanToggle(IRoomObjectAvatar avatar)
    {
        if (UsagePolicy == FurniUsagePolicy.Nobody) return false;

        if (UsagePolicy == FurniUsagePolicy.Controller)
        {
            if (avatar.RoomObjectHolder is IRoomManipulator roomManipulator)
                if (RoomObject.Room.RoomSecurityManager.GetControllerLevel(roomManipulator) >=
                    RoomControllerLevel.Rights)
                    return true;

            return false;
        }

        return true;
    }
}