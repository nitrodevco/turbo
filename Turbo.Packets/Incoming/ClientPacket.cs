using DotNetty.Buffers;
using System.Text;

namespace Turbo.Packets.Incoming
{
    public class ClientPacket : TurboPacket, IClientPacket
    {
        public ClientPacket(int header, IByteBuffer body) : base(header, body)
        {

        }

        public string PopString()
        {
            int length = Content.ReadShort();
            IByteBuffer data = Content.ReadBytes(length);
            return Encoding.UTF8.GetString(data.Array);
        }

        public int PopInt() =>
            Content.ReadInt();

        public bool PopBoolean() =>
            Content.ReadByte() == 1;

        public int RemainingLength() =>
            Content.ReadableBytes;

        public long PopLong() =>
            Content.ReadLong();

        public short PopShort() =>
            Content.ReadShort();

        public double PopDouble()
        {
            double.TryParse(PopString(), out double result);
            return result;
        }
    }
}
