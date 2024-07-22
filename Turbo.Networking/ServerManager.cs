using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Networking;
using Turbo.Networking.EventLoop;
using Turbo.Networking.Game;
using Turbo.Networking.Game.WebSocket;
using Turbo.Networking.REST;

namespace Turbo.Networking;

public class ServerManager : IServerManager
{
    public ServerManager(INetworkEventLoopGroup group, IWSGameServer wsServer, IGameServer gameServer,
        IRestServer restServer)
    {
        Servers = new List<IServer>();
        EventLoopGroup = group;

        Servers.Add(gameServer);
        Servers.Add(wsServer);
        Servers.Add(restServer);
    }

    public INetworkEventLoopGroup EventLoopGroup { get; }
    public List<IServer> Servers { get; }

    public async Task StartServersAsync()
    {
        await Task.WhenAll(Servers.Select(x => x.StartAsync()));
    }

    public async Task ShutdownServersAsync()
    {
        await Task.WhenAll(Servers.Select(x => x.ShutdownAsync()));
        await EventLoopGroup.Group.ShutdownGracefullyAsync();
    }
}