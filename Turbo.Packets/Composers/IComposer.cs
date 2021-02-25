using DotNetty.Buffers;
using Turbo.Packets.Outgoing;

namespace Turbo.Packets.Composers
{
    public interface IComposer
    {
        public int Header { get; }
        public IServerPacket WriteTo(IByteBuffer buf);
        public void Compose(IServerPacket packet);
    }
}
