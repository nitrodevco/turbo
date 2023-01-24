using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Game.Catalog.Constants;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Catalog
{
    public record GetClubOffersMessage : IMessageEvent
    {
        public ClubOfferRequestSource RequestSource { get; init; }
    }
}