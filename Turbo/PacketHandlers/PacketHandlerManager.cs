using Turbo.Core.PacketHandlers;

namespace Turbo.Main.PacketHandlers
{
    public class PacketHandlerManager : IPacketHandlerManager
    {
        private readonly IAuthenticationMessageHandler _authenticationMessageHandler;
        private readonly INavigatorMessageHandler _navigatorMessageHandler;
        private readonly IRoomAvatarMessageHandler _roomAvatarMessageHandler;
        private readonly IRoomEngineMessageHandler _roomEngineMessageHandler;
        private readonly IRoomFurnitureMessageHandler _roomFurnitureMessageHandler;
        private readonly IRoomSessionMessageHandler _roomSessionMessageHandler;
        private readonly IInventoryMessageHandler _inventoryMessageHandler;
        private readonly IUserMessageHandler _userMessageHandler;
        private readonly IWiredMessageHandler _wiredMessageHandler;
        private readonly ICatalogMessageHandler _catalogMessageHandler;

        public PacketHandlerManager(
            INavigatorMessageHandler navigatorMessageHandler,
            IRoomAvatarMessageHandler roomAvatarMessageHandler,
            IRoomEngineMessageHandler roomEngineMessageHandler,
            IRoomFurnitureMessageHandler roomFurnitureMessageHandler,
            IRoomSessionMessageHandler roomSessionMessageHandler,
            IAuthenticationMessageHandler authenticationMessageHandler,
            IInventoryMessageHandler inventoryMessageHandler,
            IUserMessageHandler userMessageHandler,
            IWiredMessageHandler wiredMessageHandler,
            ICatalogMessageHandler catalogMessageHandler)
        {
            _authenticationMessageHandler = authenticationMessageHandler;
            _navigatorMessageHandler = navigatorMessageHandler;
            _roomAvatarMessageHandler = roomAvatarMessageHandler;
            _roomFurnitureMessageHandler = roomFurnitureMessageHandler;
            _roomEngineMessageHandler = roomEngineMessageHandler;
            _roomSessionMessageHandler = roomSessionMessageHandler;
            _inventoryMessageHandler = inventoryMessageHandler;
            _userMessageHandler = userMessageHandler;
            _wiredMessageHandler = wiredMessageHandler;
            _catalogMessageHandler = catalogMessageHandler;
        }
    }
}
