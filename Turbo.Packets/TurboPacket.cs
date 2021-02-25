using DotNetty.Buffers;
using DotNetty.Common;
using System.Text;

namespace Turbo.Packets
{
    public class TurboPacket : DefaultByteBufferHolder
    {
        public int Header { get; set; }

        public TurboPacket(int header, IByteBuffer body) : base(body)
        {
            this.Header = header;
        }

        public override string ToString()
        {
            string body = Content.ToString(Encoding.UTF8);

            for (int i = 0; i < 13; i++)
            {
                body = body.Replace(((char)i).ToString(), "[" + i + "]");
            }

            return body;
        }
    }
}
