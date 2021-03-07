using Microsoft.Extensions.DependencyInjection;
using Turbo.Database.Repositories.Furniture;
using Turbo.Database.Repositories.Player;
using Turbo.Database.Repositories.Room;
using Turbo.Database.Repositories.Security;
using Turbo.Furniture;
using Turbo.Networking;
using Turbo.Networking.Clients;
using Turbo.Networking.EventLoop;
using Turbo.Networking.Game;
using Turbo.Networking.Game.Clients;
using Turbo.Networking.Game.WebSocket;
using Turbo.Networking.REST;
using Turbo.Packets;
using Turbo.Packets.Revisions;
using Turbo.Players;
using Turbo.Players.Authentication;
using Turbo.Plugins;
using Turbo.Rooms;
using Turbo.Security;

namespace Turbo.Main.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddManagers(this IServiceCollection services)
        {
            services.AddSingleton<IPluginManager, TurboPluginManager>();
            services.AddSingleton<IServerManager, ServerManager>();
            services.AddSingleton<IRevisionManager, RevisionManager>();
            services.AddSingleton<ISessionManager, SessionManager>();
            services.AddSingleton<ISecurityManager, SecurityManager>();
            services.AddSingleton<IFurnitureManager, FurnitureManager>();
            services.AddSingleton<IPlayerManager, PlayerManager>();
            services.AddSingleton<IRoomManager, RoomManager>();
            services.AddSingleton<IAuthenticationService, AuthenticationService>();
        }

        public static void AddNetworking(this IServiceCollection services)
        {
            // Servers
            services.AddSingleton<IGameServer, GameServer>();
            services.AddSingleton<IWSGameServer, WSGameServer>();
            services.AddSingleton<IRestServer, RestServer>();

            // Others
            services.AddSingleton<INetworkEventLoopGroup, NetworkEventLoopGroup>();
            services.AddSingleton<IPacketMessageHub, PacketMessageHub>();
            services.AddSingleton<ISessionFactory, SessionFactory>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IFurnitureDefinitionRepository, FurnitureDefinitionRepository>();
            services.AddSingleton<IFurnitureRepository, FurnitureRepository>();
            services.AddSingleton<IPlayerRepository, PlayerRepository>();
            services.AddSingleton<IRoomModelRepository, RoomModelRepository>();
            services.AddSingleton<IRoomRepository, RoomRepository>();
            services.AddSingleton<ISecurityTicketRepository, SecurityTicketRepository>();
        }
    }
}
