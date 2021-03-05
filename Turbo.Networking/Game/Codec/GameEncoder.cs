using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using System;
using Turbo.Packets;
using Turbo.Packets.Composers;

namespace Turbo.Networking.Game.Codec
{
    public class GameEncoder : MessageToByteEncoder<Composer>
    {
        protected override void Encode(IChannelHandlerContext context, Composer message, IByteBuffer output)
        {
            message.WriteTo(output);
        }
    }
}
