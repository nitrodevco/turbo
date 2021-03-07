using DotNetty.Buffers;
using System;
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

        public int PopInt() => Content.ReadInt();

        public bool PopBoolean() => Content.ReadByte() == 1;

        public int RemainingLength() => Content.ReadableBytes;

        public long PopLong() => Content.ReadLong();

        public short PopShort() => Content.ReadShort();

        public double PopDouble()
        {
            var doubleString = PopString();
            var parsed = double.TryParse(doubleString, out double result);

            if (parsed)
                return result;

            throw new FormatException($"'{doubleString}' is not a valid double!");
        }
    }
}
