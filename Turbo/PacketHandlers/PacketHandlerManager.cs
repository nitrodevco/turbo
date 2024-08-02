using Turbo.Core.PacketHandlers;

namespace Turbo.Main.PacketHandlers;

public class PacketHandlerManager : IPacketHandlerManager
{
    private readonly IAuthenticationMessageHandler _authenticationMessageHandler;
    private readonly ICompetitionMessageHandler _competitionMessageHandler;
    private readonly ICatalogMessageHandler _catalogMessageHandler;
    private readonly IInventoryMessageHandler _inventoryMessageHandler;
    private readonly INavigatorMessageHandler _navigatorMessageHandler;
    private readonly IRoomActionMessageHandler _roomActionMessageHandler;
    private readonly IRoomAvatarMessageHandler _roomAvatarMessageHandler;
    private readonly IRoomEngineMessageHandler _roomEngineMessageHandler;
    private readonly IRoomFurnitureMessageHandler _roomFurnitureMessageHandler;
    private readonly IRoomSessionMessageHandler _roomSessionMessageHandler;
    private readonly IRoomSettingsMessageHandler _roomSettingsMessageHandler;
    private readonly IUserMessageHandler _userMessageHandler;
    private readonly ITrackingHandler _trackingHandler;
    private readonly IAdvertisingHandler _advertisingHandler;

    public PacketHandlerManager(
        IAuthenticationMessageHandler authenticationMessageHandler,
        ICompetitionMessageHandler competitionMessageHandler,
        ICatalogMessageHandler catalogMessageHandler,
        IInventoryMessageHandler inventoryMessageHandler,
        INavigatorMessageHandler navigatorMessageHandler,
        IRoomActionMessageHandler roomActionMessageHandler,
        IRoomAvatarMessageHandler roomAvatarMessageHandler,
        IRoomEngineMessageHandler roomEngineMessageHandler,
        IRoomFurnitureMessageHandler roomFurnitureMessageHandler,
        IRoomSessionMessageHandler roomSessionMessageHandler,
        IRoomSettingsMessageHandler roomSettingsMessageHandler,
        IUserMessageHandler userMessageHandler,
        ITrackingHandler trackingHandler,
        IAdvertisingHandler advertisingHandler
    {
        _authenticationMessageHandler = authenticationMessageHandler;
        _competitionMessageHandler = competitionMessageHandler;
        _catalogMessageHandler = catalogMessageHandler;
        _inventoryMessageHandler = inventoryMessageHandler;
        _navigatorMessageHandler = navigatorMessageHandler;
        _roomActionMessageHandler = roomActionMessageHandler;
        _roomAvatarMessageHandler = roomAvatarMessageHandler;
        _roomEngineMessageHandler = roomEngineMessageHandler;
        _roomFurnitureMessageHandler = roomFurnitureMessageHandler;
        _roomSessionMessageHandler = roomSessionMessageHandler;
        _roomSettingsMessageHandler = roomSettingsMessageHandler;
        _userMessageHandler = userMessageHandler;
        _trackingHandler = trackingHandler;
        _advertisingHandler = advertisingHandler;
    }
}