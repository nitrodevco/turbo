using System.Collections.Generic;
using Turbo.Core.Game;
using Turbo.Core.Game.Furniture.Constants;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Packets.Outgoing.Room.Furniture;
using Turbo.Rooms.Object.Attributes;

namespace Turbo.Rooms.Object.Logic.Furniture;

[RoomObjectLogic("stack_helper")]
public class FurnitureStackHelperLogic : FurnitureFloorLogic
{
    public override FurniUsagePolicy UsagePolicy => FurniUsagePolicy.Controller;

    private void ResetHeight()
    {
        var currentTile = GetCurrentTile();

        if (currentTile == null || RoomObject == null) return;

        RoomObject.Z = currentTile.DefaultHeight;

        RefreshFurniture();
    }

    public void SetStackHelperHeight(IRoomObjectAvatar avatar, int height)
    {
        var tiles = GetCurrentTiles();

        if (tiles.Count == 0) return;

        var newHeight = height / 100.0;
        var defaultHeight = tiles[0].DefaultHeight;

        if (height == -100)
        {
            newHeight = defaultHeight;

            foreach (var tile in tiles)
            {
                var highestObject = tile.HighestObject;

                if (highestObject == null || !highestObject.Logic.CanStack()) continue;

                if (highestObject.Logic.Height < newHeight) continue;

                newHeight = highestObject.Logic.Height;
            }
        }

        newHeight = newHeight < defaultHeight ? defaultHeight : newHeight;

        if (newHeight > DefaultSettings.MaximumFurnitureHeight) newHeight = DefaultSettings.MaximumFurnitureHeight;

        RoomObject.Z = newHeight;

        var points = new List<IPoint>();

        foreach (var tile in tiles)
        {
            tile?.ResetHighestObject();

            points.Add(tile.Location.Clone());
        }

        RoomObject.Room?.RoomMap?.UpdatePoints(true, points.ToArray());

        if (avatar.RoomObjectHolder is ISessionHolder sessionHolder)
            sessionHolder.Session.Send(new CustomStackingHeightUpdateMessage
            {
                ItemId = RoomObject.Id,
                Height = (int)(newHeight * 100.0)
            });

        RefreshFurniture();
    }

    public override void OnMove(IRoomManipulator roomManipulator)
    {
        ResetHeight();
    }

    public override void OnInteract(IRoomObjectAvatar avatar, int param)
    {
    }

    public override bool CanStack()
    {
        return false;
    }

    public override bool CanRoll()
    {
        return false;
    }

    public override bool IsOpen(IRoomObjectAvatar avatar = null)
    {
        return false;
    }
}