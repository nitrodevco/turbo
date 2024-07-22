using Turbo.Core.Game.Furniture.Constants;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Rooms.Object.Attributes;

namespace Turbo.Rooms.Object.Logic.Furniture;

[RoomObjectLogic("gate")]
public class FurnitureGateLogic : FurnitureFloorLogic
{
    private static readonly int _gateClosedState = 0;
    private static readonly int _gateOpenState = 1;

    public override FurniUsagePolicy UsagePolicy => FurniUsagePolicy.Controller;

    public override void OnInteract(IRoomObjectAvatar avatar, int param)
    {
        var roomTile = GetCurrentTile();

        if (roomTile == null || roomTile.Avatars.Count > 0) return;

        base.OnInteract(avatar, param);
    }

    public override bool IsOpen(IRoomObjectAvatar roomObject = null)
    {
        if (StuffData.GetState() == _gateClosedState) return false;

        if (StuffData.GetState() == _gateOpenState) return true;

        return base.IsOpen(roomObject);
    }
}