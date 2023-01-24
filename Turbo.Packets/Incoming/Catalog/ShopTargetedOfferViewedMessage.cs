using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Catalog
{
    public record ShopTargetedOfferViewedMessage : IMessageEvent
    {
        public int TargetedOfferId { get; init; }
        public int TrackingState { get; init; }
    }
}