using System.Collections.Generic;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Wired
{
    public record UpdateTriggerMessage : IMessageEvent
    {
        public int ItemId { get; init; }
        public IList<int> IntegerParams { get; init; }
        public string StringParam { get; init; }
        public IList<int> SelectedItemIds { get; init; }
        public int SelectionCode { get; init; }
    }
}
