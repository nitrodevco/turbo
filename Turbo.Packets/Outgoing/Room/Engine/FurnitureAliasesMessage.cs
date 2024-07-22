using System.Collections.Generic;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Engine;

public record FurnitureAliasesMessage : IComposer
{
    public Dictionary<string, string> Aliases { get; init; }
}