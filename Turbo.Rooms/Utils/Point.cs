using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Rooms.Utils
{
    public class Point : IPoint
    {
        public int X { get; set; }
        public int Y { get; set; }
        public double Z { get; set; }

        public Point(int x = 0, int y = 0, double z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public IPoint Clone()
        {
            return new Point(X, Y, Z);
        }

        public IPoint AddPoint(IPoint point)
        {
            return AdjustPoint(point);
        }

        public IPoint SubtractPoint(IPoint point)
        {
            return AdjustPoint(new Point(-point.X, -point.Y, -point.Z));
        }

        public IPoint AdjustPoint(IPoint point)
        {
            IPoint clone = Clone();

            clone.X += point.X;
            clone.Y += point.Y;
            clone.Z += point.Z;

            return clone;
        }

        public int GetDistanceAround(IPoint point)
        {
            IPoint clone = Clone();

            clone.X -= point.X;
            clone.Y -= point.Y;

            return ((clone.X * clone.X) + (clone.Y * clone.Y));
        }

        public bool Compare(IPoint point)
        {
            if ((point == null) || (point.X != X) || (point.Y != Y)) return false;

            return true;
        }
    }
}
