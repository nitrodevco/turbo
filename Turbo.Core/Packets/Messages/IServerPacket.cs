namespace Turbo.Core.Packets.Messages;

public interface IServerPacket
{
    public int Header { get; }
    IServerPacket WriteByte(byte b);
    IServerPacket WriteByte(int b);
    IServerPacket WriteDouble(double d);
    IServerPacket WriteString(string s);
    IServerPacket WriteShort(int s);
    IServerPacket WriteInteger(int i);
    IServerPacket WriteBoolean(bool b);
    IServerPacket WriteLong(long l);
}