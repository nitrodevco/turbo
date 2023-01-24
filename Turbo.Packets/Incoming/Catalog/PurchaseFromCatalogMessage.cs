using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Catalog
{
    public record PurchaseFromCatalogMessage : IMessageEvent
    {
        public int PageId { get; init; }
        public int OfferId { get; init; }
        public string ExtraParam { get; init; }
        public int Quantity { get; init; }
    }
}