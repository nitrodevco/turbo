namespace Turbo.Core.Game.Rooms.Utils
{
    public interface IPoint
    {
        public int X { get; set; }
        public int Y { get; set; }
        public double Z { get; set; }
        public Rotation Rotation { get; set; }
        public Rotation HeadRotation { get; set; }

        public IPoint Clone();
        public IPoint AddPoint(IPoint point);
        public IPoint SubtractPoint(IPoint point);
        public IPoint AdjustPoint(IPoint point);
        public void SetRotation(Rotation? rotation);
        public int GetDistanceAround(IPoint point);
        public double GetDistanceSquared(IPoint point);
        public bool Compare(IPoint point);
        public bool CompareStrict(IPoint point);
        public Rotation CalculateHumanDirection(IPoint point);
        public Rotation CalculateWalkDirection(IPoint point);
        public Rotation CalculateHeadDirection(IPoint point);
        public Rotation CalculateSitDirection();
    }
}
