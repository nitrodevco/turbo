using Turbo.Core.PacketHandlers;

namespace Turbo.Main.PacketHandlers
{
    public class PacketHandlerManager : IPacketHandlerManager
    {
        private readonly IAuthenticationMessageHandler _authenticationMessageHandler;
        private readonly INavigatorMessageHandler _navigatorMessageHandler;
        private readonly IRoomAvatarMessageHandler _roomAvatarMessageHandler;
        private readonly IRoomEngineMessageHandler _roomEngineMessageHandler;
        private readonly IRoomSessionMessageHandler _roomSessionMessageHandler;
        public PacketHandlerManager(
            INavigatorMessageHandler navigatorMessageHandler,
            IRoomAvatarMessageHandler roomAvatarMessageHandler,
            IRoomEngineMessageHandler roomEngineMessageHandler,
            IRoomSessionMessageHandler roomSessionMessageHandler,
            IAuthenticationMessageHandler authenticationMessageHandler)
        {
            _authenticationMessageHandler = authenticationMessageHandler;
            _navigatorMessageHandler = navigatorMessageHandler;
            _roomAvatarMessageHandler = roomAvatarMessageHandler;
            _roomEngineMessageHandler = roomEngineMessageHandler;
            _roomSessionMessageHandler = roomSessionMessageHandler;
        }
    }
}
