using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Users;
public record RelationshipStatusInfoMessage : IComposer
{
    public required IPlayer Player { get; init; }
}
