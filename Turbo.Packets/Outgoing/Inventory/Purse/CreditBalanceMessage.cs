using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Inventory.Purse;
public record CreditBalanceMessage : IComposer
{
    public int credits { get; init; }
}
