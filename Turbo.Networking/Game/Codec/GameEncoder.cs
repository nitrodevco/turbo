using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using System;
using Turbo.Packets.Outgoing;

namespace Turbo.Networking.Game.Codec
{
    class GameEncoder : MessageToByteEncoder<ServerPacket>
    {
        protected override void Encode(IChannelHandlerContext context, ServerPacket message, IByteBuffer output)
        {
            output.WriteBytes(message.Content);
        }
    }
}
