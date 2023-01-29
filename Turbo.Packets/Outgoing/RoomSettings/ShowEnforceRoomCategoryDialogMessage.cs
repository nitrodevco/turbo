using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.RoomSettings
{
    public record ShowEnforceRoomCategoryDialogMessage : IComposer
    {
        public int SelectionType { get; init; }
    }
}