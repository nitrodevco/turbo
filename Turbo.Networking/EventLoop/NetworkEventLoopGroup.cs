using DotNetty.Transport.Channels;
using Turbo.Core.Configuration;

namespace Turbo.Networking.EventLoop;

public class NetworkEventLoopGroup(IEmulatorConfig config) : INetworkEventLoopGroup
{
    public IEventLoopGroup Group { get; } = new MultithreadEventLoopGroup(config.NetworkWorkerThreads);
}