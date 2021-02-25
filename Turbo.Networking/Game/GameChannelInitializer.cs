using DotNetty.Transport.Channels;
using Turbo.Networking.Game.Codec;
using Turbo.Networking.Game.Handler;

namespace Turbo.Networking.Game
{
    class GameChannelInitializer : ChannelInitializer<IChannel>
    {
        protected override void InitChannel(IChannel channel)
        {
            channel.Pipeline
                .AddLast("flashPolicy", new FlashPolicyHandler())
                .AddLast("frameEncoder", new FrameLengthFieldEncoder())
                .AddLast("frameDecoder", new FrameLengthFieldDecoder())
                .AddLast("gameEncoder", new GameEncoder())
                .AddLast("gameDecoder", new GameDecoder())
                .AddLast("messageHandler", new GameMessageHandler());
        }
    }
}
