using DotNetty.Buffers;
using System.Text;

namespace Turbo.Packets.Outgoing
{
    public class ServerPacket : TurboPacket, IServerPacket
    {
        public ServerPacket(int header, IByteBuffer body) : base(header, body)
        {
            
        }

        public void WriteByte(byte b) =>
            Content.WriteByte(b);

        public void WriteByte(int b) =>
            Content.WriteByte((byte)b);

        public void WriteDouble(double d) =>
            WriteString(d.ToString());

        public void WriteString(string s)
        {
            byte[] data = Encoding.UTF8.GetBytes(s);
            Content.WriteShort(data.Length);
            Content.WriteBytes(data);
        }

        public void WriteShort(int s) =>
            Content.WriteShort(s);

        public void WriteInteger(int i) =>
            Content.WriteInt(i);

        public void WriteBoolean(bool b) =>
            Content.WriteByte(b ? 1 : 0);

        public void WriteLong(long l) =>
            Content.WriteLong(l);
    }
}
