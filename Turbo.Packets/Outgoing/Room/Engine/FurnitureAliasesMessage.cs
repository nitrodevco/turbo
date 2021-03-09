using System.Collections.Generic;

namespace Turbo.Packets.Outgoing.Room.Engine
{
    public record FurnitureAliasesMessage : IComposer
    {
        public Dictionary<string, string> Aliases { get; init; }
    }
}
