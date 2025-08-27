using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Rooms.Object.Attributes;

namespace Turbo.Rooms.Object.Logic.Furniture;

[RoomObjectLogic("multi_height")]
public class FurnitureMultiHeightLogic : FurnitureFloorLogic
{
    public override void OnInteract(IRoomObjectAvatar avatar, int param)
    {
        var tiles = GetCurrentTiles();

        if (tiles.Count == 0) return;

        var points = new List<IPoint>();

        foreach (var tile in tiles)
        {
            if (!tile.HighestObject.Equals(RoomObject)) return;

            points.Add(tile.Location);
        }

        base.OnInteract(avatar, param);

        var room = RoomObject.Room;

        if(room is null) return;

        foreach(var tile in tiles)
        {
            tile?.ResetTileHeight();
        }

        room.RoomMap.UpdatePoints(true, points.ToArray());
    }

    public override double StackHeight
    {
        get
        {
            if (FurnitureDefinition.MultiHeights.TryGetValue(StuffData.GetState(), out var height))
            {
                return height;
            }

            return base.StackHeight;
        }
    }
}
