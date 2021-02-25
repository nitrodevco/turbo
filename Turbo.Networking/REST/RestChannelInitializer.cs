using DotNetty.Codecs.Http;
using DotNetty.Transport.Channels;
using Turbo.Networking.REST.Handler;

namespace Turbo.Networking.REST
{
    public class RestChannelInitializer : ChannelInitializer<IChannel>
    {
        protected override void InitChannel(IChannel channel)
        {
            channel.Pipeline
                .AddLast("httpCodec", new HttpServerCodec())
                .AddLast("objectAggregator", new HttpObjectAggregator(65536))
                .AddLast("RestMessageHandler", new RestMessageHandler());
        }
    }
}
