using Microsoft.Extensions.DependencyInjection;
using Turbo.Catalog;
using Turbo.Catalog.Factories;
using Turbo.Core.Game.Catalog;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Navigator;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Core.Plugins;
using Turbo.Core.Security;
using Turbo.Core.Storage;
using Turbo.Database.Queue;
using Turbo.Database.Repositories.Catalog;
using Turbo.Database.Repositories.Furniture;
using Turbo.Database.Repositories.Navigator;
using Turbo.Database.Repositories.Player;
using Turbo.Database.Repositories.Room;
using Turbo.Database.Repositories.Security;
using Turbo.Furniture;
using Turbo.Furniture.Factories;
using Turbo.Inventory.Factories;
using Turbo.Main.PacketHandlers;
using Turbo.Navigator;
using Turbo.Networking;
using Turbo.Networking.Clients;
using Turbo.Networking.EventLoop;
using Turbo.Networking.Game;
using Turbo.Networking.Game.Clients;
using Turbo.Networking.Game.WebSocket;
using Turbo.Networking.REST;
using Turbo.PacketHandlers;
using Turbo.Packets;
using Turbo.Packets.Revisions;
using Turbo.Players;
using Turbo.Players.Factories;
using Turbo.Plugins;
using Turbo.Rooms;
using Turbo.Rooms.Factories;
using Turbo.Rooms.Object;
using Turbo.Rooms.Object.Logic;
using Turbo.Security;

namespace Turbo.Main.Extensions
{
    public static class ServiceCollectionExtensions
    {
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
            services.AddTransient<IAuthenticationMessageHandler, AuthenticationMessageHandler>();
            services.AddTransient<ICatalogMessageHandler, CatalogMessageHandler>();
            services.AddTransient<IInventoryMessageHandler, InventoryMessageHandler>();
            services.AddTransient<INavigatorMessageHandler, NavigatorMessageHandler>();
            services.AddTransient<IPacketHandlerManager, PacketHandlerManager>();
            services.AddTransient<IRoomActionMessageHandler, RoomActionMessageHandler>();
            services.AddTransient<IRoomAvatarMessageHandler, RoomAvatarMessageHandler>();
            services.AddTransient<IRoomEngineMessageHandler, RoomEngineMessageHandler>();
            services.AddTransient<IRoomFurnitureMessageHandler, RoomFurnitureMessageHandler>();
            services.AddTransient<IRoomSessionMessageHandler, RoomSessionMessageHandler>();
            services.AddTransient<IRoomSettingsMessageHandler, RoomSettingsMessageHandler>();
            services.AddTransient<IUserMessageHandler, UserMessageHandler>();
            services.AddTransient<IWiredMessageHandler, WiredMessageHandler>();
        }

        public static void AddManagers(this IServiceCollection services)
        {
            services.AddSingleton<IPluginManager, TurboPluginManager>();
            services.AddSingleton<IStorageQueue, StorageQueue>();
            services.AddSingleton<IServerManager, ServerManager>();
            services.AddSingleton<IRevisionManager, RevisionManager>();
            services.AddSingleton<ISessionManager, SessionManager>();
            services.AddSingleton<ISecurityManager, SecurityManager>();
            services.AddSingleton<INavigatorManager, NavigatorManager>();
            services.AddSingleton<IFurnitureManager, FurnitureManager>();
            services.AddSingleton<ICatalogManager, CatalogManager>();
            services.AddSingleton<IPlayerManager, PlayerManager>();
            services.AddSingleton<IRoomManager, RoomManager>();
        }

        public static void AddFactories(this IServiceCollection services)
        {
            services.AddSingleton<IRoomFactory, RoomFactory>();
            services.AddSingleton<IPlayerFactory, PlayerFactory>();
            services.AddSingleton<IPlayerInventoryFactory, PlayerInventoryFactory>();
            services.AddSingleton<IRoomObjectFactory, RoomObjectFactory>();
            services.AddSingleton<IRoomObjectLogicFactory, RoomObjectLogicFactory>();
            services.AddSingleton<IFurnitureFactory, FurnitureFactory>();
            services.AddSingleton<IPlayerFurnitureFactory, PlayerFurnitureFactory>();
            services.AddSingleton<IRoomFurnitureFactory, RoomFurnitureFactory>();
            services.AddSingleton<IRoomUserFactory, RoomUserFactory>();
            services.AddSingleton<IRoomSecurityFactory, RoomSecurityFactory>();
            services.AddSingleton<IRoomWiredFactory, RoomWiredFactory>();
            services.AddSingleton<ICatalogFactory, CatalogFactory>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICatalogOfferRepository, CatalogOfferRepository>();
            services.AddScoped<ICatalogPageRepository, CatalogPageRepository>();
            services.AddScoped<ICatalogProductRepository, CatalogProductRepository>();
            services.AddScoped<IFurnitureDefinitionRepository, FurnitureDefinitionRepository>();
            services.AddScoped<IFurnitureRepository, FurnitureRepository>();
            services.AddScoped<IPlayerBadgeRepository, PlayerBadgeRepository>();
            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<IRoomBanRepository, RoomBanRepository>();
            services.AddScoped<IRoomModelRepository, RoomModelRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IRoomRightRepository, RoomRightRepository>();
            services.AddScoped<ISecurityTicketRepository, SecurityTicketRepository>();
            services.AddScoped<INavigatorEventCategoryRepository, NavigatorEventCategoryRepository>();
        }
    }
}
