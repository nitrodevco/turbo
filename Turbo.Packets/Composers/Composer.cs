using DotNetty.Buffers;
using Turbo.Packets.Outgoing;

namespace Turbo.Packets.Composers
{
    public abstract class Composer : IComposer
    {
        public int Header { get; }

        public Composer(int header)
        {
            this.Header = header;
        }

        public IServerPacket WriteTo(IByteBuffer buf)
        {
            IServerPacket packet = new ServerPacket(Header, buf);

            packet.WriteShort(Header);

            this.Compose(packet);
            
            return packet;
        }

        public abstract void Compose(IServerPacket packet);
    }
}
