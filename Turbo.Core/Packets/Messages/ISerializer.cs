using DotNetty.Buffers;

namespace Turbo.Core.Packets.Messages;

public interface ISerializer
{
    public int Header { get; }
    public IServerPacket Serialize(IByteBuffer output, IComposer message);
}