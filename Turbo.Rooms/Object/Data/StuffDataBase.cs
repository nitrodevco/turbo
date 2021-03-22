using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Turbo.Core.Game.Rooms.Object.Data;

namespace Turbo.Rooms.Object.Data
{
    public class StuffDataBase : IStuffData
    {
        public int Flags { get; set; }
        public int UniqueNumber { get; set; }
        public int UniqueSeries { get; set; }

        public StuffDataBase()
        {

        }

        public virtual bool InitializeFromFurnitureData(string data)
        {
            int uniqueNumber = 0;
            int uniqueSeries = 0;

            if ((uniqueNumber > 0) && (uniqueSeries > 0))
            {
                Flags += (int)StuffDataFlags.UniqueSet;
            }

            return true;
        }

        public virtual string GetLegacyString()
        {
            return "";
        }

        public virtual void SetState(string data)
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

        public string ToJson()
        {
            string json;
            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream))
                {
                    if ((Flags & (int)StuffDataFlags.UniqueSet) > 0)
                    {
                        writer.WriteNumber("uniqueNumber", UniqueNumber);
                        writer.WriteNumber("uniqueSeries", UniqueSeries);
                    }
                    SerializeJson(writer, stream);
                    writer.WriteEndObject();
                }

                json = Encoding.UTF8.GetString(stream.ToArray());
            }
            return json;
        }

        public virtual void SerializeJson(Utf8JsonWriter writer, MemoryStream stream)
        {

        }
    }
}
