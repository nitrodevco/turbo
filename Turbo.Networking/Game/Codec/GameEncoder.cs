using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Turbo.Packets.Outgoing;

namespace Turbo.Networking.Game.Codec;

public class GameEncoder : MessageToByteEncoder<ServerPacket>
{
    protected override void Encode(IChannelHandlerContext context, ServerPacket message, IByteBuffer output)
    {
        output.WriteBytes(message.Content);
    }
}