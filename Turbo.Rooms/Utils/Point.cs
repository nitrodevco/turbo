﻿using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Rooms.Utils
{
    public class Point : IPoint
    {
        public int X { get; set; }
        public int Y { get; set; }
        public double Z { get; set; }
        public Rotation Rotation { get; set; }
        public Rotation HeadRotation { get; set; }

        public Point(int x = 0, int y = 0, double z = 0, Rotation rotation = Rotation.North, Rotation headRotation = Rotation.North)
        {
            X = x;
            Y = y;
            Z = z;
            Rotation = rotation;
            HeadRotation = headRotation;
        }

        public IPoint Clone()
        {
            return new Point(X, Y, Z, Rotation, HeadRotation);
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

        public void SetRotation(Rotation? rotation)
        {
            if (rotation == null) return;

            Rotation = (Rotation) rotation;
            HeadRotation = (Rotation) rotation;
        }

        public int GetDistanceAround(IPoint point)
        {
            IPoint clone = Clone();

            clone.X -= point.X;
            clone.Y -= point.Y;

            return (clone.X * clone.X) + (clone.Y * clone.Y);
        }

        public bool Compare(IPoint point)
        {
            if ((point == null) || (point.X != X) || (point.Y != Y)) return false;

            return true;
        }

        public bool CompareStrict(IPoint point)
        {
            if ((point == null) || (point.X != X) || (point.Y != Y) || (point.Rotation != Rotation)) return false;

            return true;
        }

        public Rotation CalculateHumanDirection(IPoint point)
        {
            if (point == null) return Rotation.North;

            if ((X > point.X) && (Y > point.Y)) return Rotation.NorthWest;

            if ((X < point.X) && (Y < point.Y)) return Rotation.SouthEast;

            if ((X > point.X) && (Y < point.Y)) return Rotation.SouthWest;

            if ((X < point.X) && (Y > point.Y)) return Rotation.NorthEast;

            if (X > point.X) return Rotation.West;

            if (X < point.X) return Rotation.East;

            if (Y < point.Y) return Rotation.South;

            return Rotation.North;
        }

        public Rotation CalculateWalkDirection(IPoint point)
        {
            if (point == null) return Rotation.NorthEast;

            if (X == point.X)
            {
                if (Y < point.Y) return Rotation.South;
                else return Rotation.North;
            }

            else if (X > point.X)
            {
                if (Y == point.Y) return Rotation.West;
                else if (Y < point.Y) return Rotation.SouthWest;
                else return Rotation.NorthWest;
            }

            else if (Y == point.Y) return Rotation.East;

            else if (Y < point.Y) return Rotation.SouthEast;

            return Rotation.NorthEast;
        }

        public Rotation CalculateHeadDirection(IPoint point)
        {
            if ((point == null) || ((int)Rotation % 2 == 0)) return Rotation;

            int difference = ((int)Rotation - (int)CalculateHumanDirection(point));

            if (difference > 0) return Rotation - 1;

            if (difference < 0) return Rotation + 1;

            return Rotation;
        }

        public Rotation CalculateSitDirection()
        {
            return (((int)Rotation % 2) != 0) ? (Rotation - 1) : Rotation;
        }

        public override string ToString()
        {
            return X + "," + Y + "," + Z;
        }
    }
}
