namespace Turbo.Packets.Outgoing
{
    public interface IServerPacket
    {
        void WriteByte(byte b);
        void WriteByte(int b);
        void WriteDouble(double d);
        void WriteString(string s);
        void WriteShort(int s);
        void WriteInteger(int i);
        void WriteBoolean(bool b);
        void WriteLong(long l);
        public int Header { get; }
    }
}
