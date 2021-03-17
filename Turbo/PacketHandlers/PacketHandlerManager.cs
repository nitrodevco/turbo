using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.PacketHandlers;

namespace Turbo.Main.PacketHandlers
{
    public class PacketHandlerManager : IPacketHandlerManager
    {
        public PacketHandlerManager(
            INavigatorMessageHandler navigatorMessageHandler,
            IRoomAvatarMessageHandler roomAvatarMessageHandler,
            IRoomEngineMessageHandler roomMessageHandler,
            IRoomSessionMessageHandler roomSessionMessageHandler)
        {

        }
    }
}
