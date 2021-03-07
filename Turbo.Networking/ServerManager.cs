using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Networking.EventLoop;
using Turbo.Networking.Game;
using Turbo.Networking.Game.WebSocket;
using Turbo.Networking.REST;

namespace Turbo.Networking
{
    public class ServerManager : IServerManager
    {
        public List<IServer> Servers { get; }

        public INetworkEventLoopGroup EventLoopGroup { get; }

        public ServerManager(INetworkEventLoopGroup group, IWSGameServer wsServer, IGameServer gameServer, IRestServer restServer)
        {
            Servers = new List<IServer>();
            EventLoopGroup = group;

            Servers.Add(gameServer);
            Servers.Add(wsServer);
            Servers.Add(restServer);
        }

        public async Task StartServersAsync()
        {
            foreach (IServer server in Servers)
            {
                await server.StartAsync();
            }
        }

        public async Task ShutdownServersAsync()
        {
            foreach (IServer server in Servers)
            {
                await server.ShutdownAsync();
            }
            await EventLoopGroup.Group.ShutdownGracefullyAsync();
        }
    }
}
