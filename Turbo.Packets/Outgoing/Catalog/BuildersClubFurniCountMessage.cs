using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Catalog
{
    public record BuildersClubFurniCountMessage : IComposer
    {
        public int FurniCount { get; init; }
    }
}