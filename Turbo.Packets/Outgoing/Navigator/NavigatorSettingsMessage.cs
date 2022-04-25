using System.Collections.Generic;
using Turbo.Core.Packets.Messages;
using Turbo.Packets.Shared.Navigator;

namespace Turbo.Packets.Outgoing.Navigator
{
    public record NavigatorSettingsMessage : IComposer
    {
        public int HomeRoomId { get; init; }

        public int RoomIdToEnter {  get; init; }
    }
}
