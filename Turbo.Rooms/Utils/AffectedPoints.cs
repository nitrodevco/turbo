using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Rooms.Utils
{
    public class AffectedPoints
    {
        public static IList<IPoint> GetPoints(IRoomObject roomObject, IPoint point = null)
        {
            IList<IPoint> points = new List<IPoint>();

            point ??= roomObject.Location;

            if(roomObject.Logic is IFurnitureLogic furnitureLogic)
            {
                int width = furnitureLogic.FurnitureDefinition.X;
                int length = furnitureLogic.FurnitureDefinition.Y;
                Rotation rotation = point.Rotation;

                if((rotation == Rotation.East) || (rotation == Rotation.West))
                {
                    int tempWidth = width;

                    width = length;
                    length = tempWidth;
                }

                for(int x = point.X; x < point.X + width; x++)
                {
                    for(int y = point.Y; y < point.Y + length; y++)
                    {
                        points.Add(new Point(x, y));
                    }
                }
            }

            return points;
        }

        public static IList<IPoint> GetPillowPoints(IRoomObject roomObject, IPoint point = null)
        {
            IList<IPoint> points = new List<IPoint>();

            point ??= roomObject.Location;

            points.Add(new Point(point.X, point.Y));

            if (roomObject.Logic is IFurnitureLogic furnitureLogic)
            {
                if (furnitureLogic.FurnitureDefinition.Y == 1) return points;

                Rotation rotation = point.Rotation;

                int x = rotation == Rotation.North ? point.X + 1 : point.X;
                int y = rotation == Rotation.East ? point.Y + 1 : point.Y;

                points.Add(new Point(x, y));
            }

            return points;
        }
    }
}
