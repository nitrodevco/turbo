using Microsoft.Extensions.DependencyInjection;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Core.Packets;
using Turbo.Core.Plugins;
using Turbo.Core.Security;
using Turbo.Core.Security.Authentication;
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
using Turbo.Plugins;
using Turbo.Rooms;
using Turbo.Rooms.Object;
using Turbo.Rooms.Object.Logic;
using Turbo.Security;
using Turbo.Security.Authentication;

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
            services.AddSingleton<IRoomObjectLogicFactory, RoomObjectLogicFactory>();
            services.AddSingleton<IRoomObjectFactory, RoomObjectFactory>();
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

            // Packet Handlers
            services.AddTransient<IRoomMessageHandler, RoomMessageHandler>();
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
