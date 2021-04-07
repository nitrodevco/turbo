using System.Collections.Generic;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Wired
{
    public record UpdateActionMessage : IMessageEvent
    {
        public int ItemId { get; init; }
        public IList<int> IntegerParms { get; init; }
        public string StringParam { get; init; }
        public IList<int> SelectedItemIds { get; init; }
        public int Delay { get; init; }
        public int SelectionCode { get; init; }
    }
}
