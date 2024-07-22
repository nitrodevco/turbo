using System;
using DotNetty.Codecs.Http;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Turbo.Networking.REST.Handler;

namespace Turbo.Networking.REST;

public class RestChannelInitializer : ChannelInitializer<IChannel>
{
    private readonly IServiceProvider _provider;

    public RestChannelInitializer(IServiceProvider provider)
    {
        _provider = provider;
    }

    protected override void InitChannel(IChannel channel)
    {
        channel.Pipeline
            .AddLast("httpCodec", new HttpServerCodec())
            .AddLast("objectAggregator", new HttpObjectAggregator(65536))
            .AddLast("RestMessageHandler", new RestMessageHandler(_provider.GetService<ILogger<RestMessageHandler>>()));
    }
}