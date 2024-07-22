using System;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Rooms.Utils;

public class Point : IPoint
{
    public Point(int x = 0, int y = 0, double z = 0, Rotation rotation = Rotation.North,
        Rotation headRotation = Rotation.North)
    {
        X = x;
        Y = y;
        Z = z;
        Rotation = rotation;
        HeadRotation = headRotation;
    }

    public Point(IPoint point)
    {
        X = point.X;
        Y = point.Y;
        Z = point.Z;
        Rotation = point.Rotation;
        HeadRotation = point.HeadRotation;
    }

    public int X { get; set; }
    public int Y { get; set; }
    public double Z { get; set; }
    public Rotation Rotation { get; set; }
    public Rotation HeadRotation { get; set; }

    public void SetRotation(Rotation? rotation)
    {
        if (rotation == null) return;

        Rotation = (Rotation)rotation;
        HeadRotation = (Rotation)rotation;
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
        var clone = Clone();

        clone.X += point.X;
        clone.Y += point.Y;
        clone.Z += point.Z;

        return clone;
    }

    public IPoint GetPointForward(int offset = 1)
    {
        return GetPoint(Rotation, offset);
    }

    public IPoint GetPoint(Rotation rotation, int offset = 1)
    {
        var clone = Clone();

        rotation = (Rotation)((int)rotation % 8);

        switch (rotation)
        {
            case Rotation.North:
                clone.Y -= offset;
                break;
            case Rotation.NorthEast:
                clone.X += offset;
                clone.Y -= offset;
                break;
            case Rotation.East:
                clone.X += offset;
                break;
            case Rotation.SouthEast:
                clone.X += offset;
                clone.Y += offset;
                break;
            case Rotation.South:
                clone.Y += offset;
                break;
            case Rotation.SouthWest:
                clone.X -= offset;
                clone.Y += offset;
                break;
            case Rotation.West:
                clone.X -= offset;
                break;
            case Rotation.NorthWest:
                clone.X -= offset;
                clone.Y -= offset;
                break;
        }

        return clone;
    }

    public int GetDistanceAround(IPoint point)
    {
        var clone = Clone();

        clone.X -= point.X;
        clone.Y -= point.Y;

        return Math.Abs(clone.X * clone.X) + Math.Abs(clone.Y * clone.Y);
    }

    public double GetDistanceSquared(IPoint point)
    {
        var clone = Clone();

        clone.X -= point.X;
        clone.Y -= point.Y;

        return Math.Sqrt(clone.X * clone.X + clone.Y * clone.Y);
    }

    public bool Compare(IPoint point)
    {
        if (point == null || point.X != X || point.Y != Y) return false;

        return true;
    }

    public bool CompareStrict(IPoint point)
    {
        if (point == null || point.X != X || point.Y != Y || point.Rotation != Rotation) return false;

        return true;
    }

    public Rotation CalculateHumanRotation(IPoint point)
    {
        if (point == null) return Rotation.North;

        if (X > point.X && Y > point.Y) return Rotation.NorthWest;

        if (X < point.X && Y < point.Y) return Rotation.SouthEast;

        if (X > point.X && Y < point.Y) return Rotation.SouthWest;

        if (X < point.X && Y > point.Y) return Rotation.NorthEast;

        if (X > point.X) return Rotation.West;

        if (X < point.X) return Rotation.East;

        if (Y < point.Y) return Rotation.South;

        return Rotation.North;
    }

    public Rotation CalculateWalkRotation(IPoint point)
    {
        if (point == null) return Rotation.NorthEast;

        if (X == point.X)
        {
            if (Y < point.Y) return Rotation.South;
            return Rotation.North;
        }

        if (X > point.X)
        {
            if (Y == point.Y) return Rotation.West;
            if (Y < point.Y) return Rotation.SouthWest;
            return Rotation.NorthWest;
        }

        if (Y == point.Y)
            return Rotation.East;

        if (Y < point.Y) return Rotation.SouthEast;

        return Rotation.NorthEast;
    }

    public Rotation CalculateHeadRotation(IPoint point)
    {
        if (point == null || (int)Rotation % 2 > 0) return Rotation;

        var difference = (int)Rotation - (int)CalculateHumanRotation(point);

        if (difference > 0) return Rotation - 1;

        if (difference < 0) return Rotation + 1;

        return Rotation;
    }

    public Rotation CalculateSitRotation()
    {
        return (int)Rotation % 2 > 0 ? Rotation - 1 : Rotation;
    }

    public override string ToString()
    {
        return X + "," + Y + "," + Z;
    }
}