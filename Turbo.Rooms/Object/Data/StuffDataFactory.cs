using Turbo.Core.Game.Rooms.Object.Data;
using Turbo.Rooms.Object.Data.Types;

namespace Turbo.Rooms.Object.Data
{
    public class StuffDataFactory
    {
        public IStuffData CreateStuffData(int flags)
        {
            IStuffData stuffData = null;

            switch (flags & 0xFF)
            {
                case (int)StuffDataKey.LegacyKey:
                    stuffData = new LegacyStuffData();
                    break;
                case (int)StuffDataKey.MapKey:
                    stuffData = new MapStuffData();
                    break;
                case (int)StuffDataKey.StringKey:
                    stuffData = new StringStuffData();
                    break;
                case (int)StuffDataKey.VoteKey:
                    stuffData = new VoteStuffData();
                    break;
                case (int)StuffDataKey.EmptyKey:
                    stuffData = new EmptyStuffData();
                    break;
                case (int)StuffDataKey.NumberKey:
                    stuffData = new NumberStuffData();
                    break;
                case (int)StuffDataKey.HighscoreKey:
                    stuffData = new HighscoreStuffData();
                    break;
                case (int)StuffDataKey.CrackableKey:
                    stuffData = new CrackableStuffData();
                    break;
            }

            if (stuffData == null) return null;

            stuffData.Flags = flags;

            return stuffData;
        }
    }
}
