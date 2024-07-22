using System.Collections.Generic;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Chat;

public record RoomFilterSettingsMessage : IComposer
{
    public IList<string> BadWords { get; init; }
}