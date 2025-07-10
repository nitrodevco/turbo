using System.Text;
using DotNetty.Buffers;

namespace Turbo.Packets;

public class TurboPacket : DefaultByteBufferHolder
{
    public TurboPacket(int header, IByteBuffer body) : base(body)
    {
        Header = header;
    }

    public int Header { get; set; }

    public override string ToString()
    {
        if (Content == null || Content.ReferenceCount <= 0)
            return "[Released buffer]";

        var copy = Content.Copy();

        try
        {
            var body = copy.ToString(Encoding.UTF8);

            for (var i = 0; i < 13; i++)
                body = body.Replace(((char)i).ToString(), $"[{i}]");

            return body;
        }
        finally
        {
            copy.Release();
        }
    }
}