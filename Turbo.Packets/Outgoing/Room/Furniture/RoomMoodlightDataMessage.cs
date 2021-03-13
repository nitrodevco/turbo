using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Furniture;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Handshake
{
    public record RoomMoodlightDataMessage : IComposer
    {
        public int SelectedPresetId { get; init; }
        public IList<RoomMoodlightData> Presets { get; init; }
    }
}
