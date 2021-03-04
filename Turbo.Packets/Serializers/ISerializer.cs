using DotNetty.Buffers;
using Turbo.Packets.Outgoing;

namespace Turbo.Packets.Serializers
{
    public interface ISerializer
    {
        public int Header { get; }
        public IServerPacket Serialize(IByteBuffer output, IComposer message);
    }
}
