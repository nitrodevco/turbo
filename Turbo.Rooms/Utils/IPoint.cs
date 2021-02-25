using System;
namespace Turbo.Rooms.Utils
{
    public interface IPoint
    {
        public int X { get; set; }
        public int Y { get; set; }
        public double Z { get; set; }

        public IPoint Clone();
        public IPoint AddPoint(IPoint point);
        public IPoint SubtractPoint(IPoint point);
        public IPoint AdjustPoint(IPoint point);
    }
}
