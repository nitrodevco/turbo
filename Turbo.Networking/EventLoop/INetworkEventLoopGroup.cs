using DotNetty.Transport.Channels;

namespace Turbo.Networking.EventLoop;

public interface INetworkEventLoopGroup
{
    public IEventLoopGroup Group { get; }
}