using DotNetty.Codecs.Http;
using DotNetty.Codecs.Http.WebSockets;
using DotNetty.Transport.Channels;
using Turbo.Networking.Game.Codec;
using Turbo.Networking.Game.Handler;
using Turbo.Networking.Game.WebSocket.Codec;

namespace Turbo.Networking.Game.WebSocket
{
    public class WSChannelInitializer : ChannelInitializer<IChannel>
    {
        protected override void InitChannel(IChannel channel)
        {
            channel.Pipeline
                .AddLast("httpCodec", new HttpServerCodec())
                .AddLast("objectAggregator", new HttpObjectAggregator(65536))
                .AddLast("wsProtocolHandler", new WebSocketServerProtocolHandler("/", true))
                .AddLast("websocketCodec", new WebSocketCodec())
                .AddLast("frameEncoder", new FrameLengthFieldEncoder())
                .AddLast("frameDecoder", new FrameLengthFieldDecoder())
                .AddLast("gameEncoder", new GameEncoder())
                .AddLast("gameDecoder", new GameDecoder())
                .AddLast("messageHandler", new GameMessageHandler());
        }
    }
}
