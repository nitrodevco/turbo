using Turbo.Core.Packets.Messages;

namespace Turbo.Rooms.Object.Data.Types
{
    public class LegacyStuffData : StuffDataBase
    {
        public string Data { get; set; }

        public override string GetLegacyString()
        {
            return Data == null ? "" : Data;
        }

        public override void SetState(string state)
        {
            Data = state;
        }
    }
}
