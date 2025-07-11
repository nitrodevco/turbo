using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Turbo.Catalog;
using Turbo.Catalog.Factories;
using Turbo.Core.Configuration;
using Turbo.Core.Database.Factories.Catalog;
using Turbo.Core.Database.Factories.Furniture;
using Turbo.Core.Database.Factories.Players;
using Turbo.Core.Database.Factories.Rooms;
using Turbo.Core.EventHandlers;
using Turbo.Core.Events;
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
using Turbo.Database.Repositories.ChatStyles;
using Turbo.Database.Repositories.Furniture;
using Turbo.Database.Repositories.Navigator;
using Turbo.Database.Repositories.Player;
using Turbo.Database.Repositories.Room;
using Turbo.Database.Repositories.Security;
using Turbo.Database.Repositories.Tracking;
using Turbo.EventHandlers;
using Turbo.Events;
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

namespace Turbo.Main.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddEncryption(this IServiceCollection services, IConfiguration configuration)
    {
        var rsaSettings = new RsaSettings();

        configuration
            .GetSection("RsaSettings")
            .Bind(rsaSettings);

        // add configuration
        services.AddSingleton<IRsaService, RsaService>(_ => new RsaService(
            rsaSettings.KeySize,
            rsaSettings.PublicKey,
            rsaSettings.PrivateKey
        ));
        services.AddSingleton<IDiffieService, DiffieService>();
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
        services.AddSingleton<ITurboEventHub, TurboEventHub>();

        // Event Handlers
        services.AddTransient<IEventHandlerManager, EventHandlerManager>();
        services.AddTransient<IEventHandler, EventHandler>();
        services.AddTransient<IEventHandler, UserLoginEventHandler>();

        // Packet Handlers
        services.AddTransient<IPacketHandlerManager, AuthenticationMessageHandler>();
        services.AddTransient<IPacketHandlerManager, CompetitionPacketHandlers>();
        services.AddTransient<IPacketHandlerManager, CatalogMessageHandler>();
        services.AddTransient<IPacketHandlerManager, InventoryMessageHandler>();
        services.AddTransient<IPacketHandlerManager, NavigatorMessageHandler>();
        services.AddTransient<IPacketHandlerManager, RoomActionMessageHandler>();
        services.AddTransient<IPacketHandlerManager, RoomAvatarMessageHandler>();
        services.AddTransient<IPacketHandlerManager, RoomEngineMessageHandler>();
        services.AddTransient<IPacketHandlerManager, RoomFurnitureMessageHandler>();
        services.AddTransient<IPacketHandlerManager, RoomSessionMessageHandler>();
        services.AddTransient<IPacketHandlerManager, RoomSettingsMessageHandler>();
        services.AddTransient<IPacketHandlerManager, UserMessageHandler>();
        services.AddTransient<IPacketHandlerManager, TrackingHandler>();
        services.AddTransient<IPacketHandlerManager, AdvertisementMessageHandler>();
        services.AddTransient<IPacketHandlerManager, CameraMessageHandler>();
    }

    public static void AddManagers(this IServiceCollection services)
    {
        services.AddSingleton<IPluginManager, TurboPluginManager>();
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
        services.AddSingleton<ICatalogFactory, CatalogFactory>();
        services.AddSingleton<IRoomChatFactory, RoomChatFactory>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IStorageQueue, StorageQueue>();
        services.AddScoped<ICatalogOfferRepository, CatalogOfferRepository>();
        services.AddScoped<ICatalogPageRepository, CatalogPageRepository>();
        services.AddScoped<ICatalogProductRepository, CatalogProductRepository>();
        services.AddScoped<IFurnitureDefinitionRepository, FurnitureDefinitionRepository>();
        services.AddScoped<IFurnitureRepository, FurnitureRepository>();
        services.AddScoped<IPlayerBadgeRepository, PlayerBadgeRepository>();
        services.AddScoped<IPlayerCurrencyRepository, PlayerCurrencyRepository>();
        services.AddScoped<IPlayerRepository, PlayerRepository>();
        services.AddScoped<IRoomBanRepository, RoomBanRepository>();
        services.AddScoped<IRoomChatlogRepository, RoomChatlogRepository>();
        services.AddScoped<IRoomModelRepository, RoomModelRepository>();
        services.AddScoped<IRoomMuteRepository, RoomMuteRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IRoomRightRepository, RoomRightRepository>();
        services.AddScoped<IRoomEntryLogRepository, RoomEntryLogRepository>();
        services.AddScoped<ISecurityTicketRepository, SecurityTicketRepository>();
        services.AddScoped<INavigatorRepository, NavigatorRepository>();
        services.AddScoped<IPlayerChatStyleRepository, PlayerChatStyleRepository>();
        services.AddScoped<IPlayerChatStyleOwnedRepository, PlayerChatStyleOwnedRepository>();
        services.AddScoped<IPerformanceLogRepository, PerformanceLogRepository>();
        services.AddScoped<IPlayerPerksRepository, PlayerPerksRepository>();
        services.AddScoped<IPlayerFavouriteRoomsRepository, PlayerFavouriteRoomsRepository>();
    }
}