using Turbo.Core.Game.Wired.Data;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Wired;

public record WiredDataMessage : IComposer
{
    public IWiredData WiredData { get; init; }
}