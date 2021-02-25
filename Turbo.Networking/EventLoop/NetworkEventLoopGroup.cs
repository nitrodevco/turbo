using DotNetty.Transport.Channels;
using Turbo.Core.Configuration;

namespace Turbo.Networking.EventLoop
{
    public class NetworkEventLoopGroup : INetworkEventLoopGroup
    {
        private readonly IEmulatorConfig _config;
        public IEventLoopGroup Group { get; }

        public NetworkEventLoopGroup(
            IEmulatorConfig config)
        {
            this._config = config;
            this.Group = new MultithreadEventLoopGroup(_config.NetworkWorkerThreads);
        }
    }
}
