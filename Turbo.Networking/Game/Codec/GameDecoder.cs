using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using System.Collections.Generic;
using Turbo.Core.Packets.Messages;
using Turbo.Packets.Incoming;

namespace Turbo.Networking.Game.Codec
{
    public class GameDecoder : MessageToMessageDecoder<IByteBuffer>
    {
        protected override void Decode(IChannelHandlerContext context, IByteBuffer message, List<object> output)
        {
            short header = message.ReadShort();
            IClientPacket packet = new ClientPacket(header, message.ReadBytes(message.ReadableBytes));
            output.Add(packet);
        }
    }
}
