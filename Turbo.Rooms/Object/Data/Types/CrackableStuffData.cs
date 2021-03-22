using System.IO;
using System.Text.Json;

namespace Turbo.Rooms.Object.Data.Types
{
    public class CrackableStuffData : StuffDataBase
    {
        public override void SerializeJson(Utf8JsonWriter writer, MemoryStream stream)
        {
            base.SerializeJson(writer, stream);
        }
    }
}
