using System.Text.Json.Serialization;
using Turbo.Core.Game.Rooms.Object.Data;
using Turbo.Core.Packets.Messages;

namespace Turbo.Rooms.Object.Data
{
    public class StuffDataBase : IStuffData
    {
        [JsonIgnore]
        public int Flags { get; set; }
        public int UniqueNumber { get; set; }
        public int UniqueSeries { get; set; }

        public StuffDataBase()
        {
            //if (IsUnique)
            //{
            //    Flags += (int)StuffDataFlags.UniqueSet;
            //}
        }

        public virtual void WriteToPacket(IServerPacket packet)
        {
            if (!IsUnique()) return;

            packet.WriteInteger(UniqueNumber);
            packet.WriteInteger(UniqueSeries);
        }

        public virtual string GetLegacyString()
        {
            return "";
        }

        public virtual void SetState(string state)
        {
            return;
        }

        public int GetState()
        {
            int state = int.Parse(GetLegacyString());

            return state;
        }

        public bool IsUnique()
        {
            return UniqueSeries > 0;
        }
    }
}
