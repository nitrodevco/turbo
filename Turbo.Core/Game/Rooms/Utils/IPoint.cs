namespace Turbo.Core.Game.Rooms.Utils;

public interface IPoint
{
    public int X { get; set; }
    public int Y { get; set; }
    public double Z { get; set; }
    public Rotation Rotation { get; set; }
    public Rotation HeadRotation { get; set; }

    public void SetRotation(Rotation? rotation);
    public IPoint Clone();
    public IPoint AddPoint(IPoint point);
    public IPoint SubtractPoint(IPoint point);
    public IPoint AdjustPoint(IPoint point);
    public IPoint GetPointForward(int offset = 1);
    public IPoint GetPoint(Rotation rotation, int offset = 1);
    public int GetDistanceAround(IPoint point);
    public double GetDistanceSquared(IPoint point);
    public bool Compare(IPoint point);
    public bool CompareStrict(IPoint point);
    public Rotation CalculateHumanRotation(IPoint point);
    public Rotation CalculateWalkRotation(IPoint point);
    public Rotation CalculateHeadRotation(IPoint point);
    public Rotation CalculateSitRotation();
}