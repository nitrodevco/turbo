using System.Text;
using DotNetty.Buffers;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing;

public class ServerPacket : TurboPacket, IServerPacket
{
    public ServerPacket(int header, IByteBuffer body) : base(header, body)
    {
    }

    public IServerPacket WriteByte(byte b)
    {
        Content.WriteByte(b);
        return this;
    }

    public IServerPacket WriteByte(int b)
    {
        Content.WriteByte((byte)b);
        return this;
    }

    public IServerPacket WriteDouble(double d)
    {
        Content.WriteDouble(d);
        return this;
    }

    public IServerPacket WriteString(string s)
    {
        var data = Encoding.UTF8.GetBytes(s ?? string.Empty);
        Content.WriteShort(data.Length);
        Content.WriteBytes(data);

        return this;
    }

    public IServerPacket WriteShort(int s)
    {
        Content.WriteShort(s);
        return this;
    }

    public IServerPacket WriteInteger(int i)
    {
        Content.WriteInt(i);
        return this;
    }

    public IServerPacket WriteBoolean(bool b)
    {
        Content.WriteByte(b ? 1 : 0);
        return this;
    }

    public IServerPacket WriteLong(long l)
    {
        Content.WriteLong(l);
        return this;
    }
}