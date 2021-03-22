using DotNetty.Transport.Channels;
using Turbo.Core.Configuration;

namespace Turbo.Networking.EventLoop
{
    public class NetworkEventLoopGroup : INetworkEventLoopGroup
    {
        public IEventLoopGroup Group { get; }

        public NetworkEventLoopGroup(
            IEmulatorConfig config)
        {
            Group = new MultithreadEventLoopGroup(config.NetworkWorkerThreads);
        }
    }
}
