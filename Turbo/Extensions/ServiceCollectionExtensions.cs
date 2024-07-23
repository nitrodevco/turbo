using Microsoft.Extensions.DependencyInjection;
using Turbo.Catalog;
using Turbo.Catalog.Factories;
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

namespace Turbo.Main.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddEncryption(this IServiceCollection services)
    {
        // add configuration
        services.AddSingleton<IRsaService, RsaService>(_ => new RsaService(
            "3",
            "86851dd364d5c5cece3c883171cc6ddc5760779b992482bd1e20dd296888df91b33b936a7b93f06d29e8870f703a216257dec7c81de0058fea4cc5116f75e6efc4e9113513e45357dc3fd43d4efab5963ef178b78bd61e81a14c603b24c8bcce0a12230b320045498edc29282ff0603bc7b7dae8fc1b05b52b2f301a9dc783b7",
            "59ae13e243392e89ded305764bdd9e92e4eafa67bb6dac7e1415e8c645b0950bccd26246fd0d4af37145af5fa026c0ec3a94853013eaae5ff1888360f4f9449ee023762ec195dff3f30ca0b08b8c947e3859877b5d7dced5c8715c58b53740b84e11fbc71349a27c31745fcefeeea57cff291099205e230e0c7c27e8e1c0512b")
        );
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
        services.AddTransient<IAuthenticationMessageHandler, AuthenticationMessageHandler>();
        services.AddTransient<ICompetitionMessageHandler, CompetitionPacketHandlers>();
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
        services.AddScoped<ISecurityTicketRepository, SecurityTicketRepository>();
        services.AddScoped<INavigatorRepository, NavigatorRepository>();
        services.AddScoped<IPlayerChatStyleRepository, PlayerChatStyleRepository>();
        services.AddScoped<IPlayerChatStyleOwnedRepository, PlayerChatStyleOwnedRepository>();
    }
}