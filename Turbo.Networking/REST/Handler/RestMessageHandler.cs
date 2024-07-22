using DotNetty.Codecs.Http;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;

namespace Turbo.Networking.REST.Handler;

public class RestMessageHandler : SimpleChannelInboundHandler<IFullHttpRequest>
{
    private readonly ILogger<RestMessageHandler> _logger;

    public RestMessageHandler(ILogger<RestMessageHandler> logger)
    {
        _logger = logger;
    }

    protected override void ChannelRead0(IChannelHandlerContext ctx, IFullHttpRequest msg)
    {
        if (msg.Method.Equals(HttpMethod.Get))
        {
        }

        _logger.LogDebug("Received http message " + msg.Method + msg);
    }
}