using System.Text;
using DotNetty.Buffers;

namespace Turbo.Packets;

public class TurboPacket : DefaultByteBufferHolder
{
    protected readonly StringBuilder _log = new();

    public TurboPacket(int header, IByteBuffer body) : base(body)
    {
        Header = header;
    }

    public int Header { get; set; }

    public override string ToString()
    {
        return _log.ToString();
    }
}