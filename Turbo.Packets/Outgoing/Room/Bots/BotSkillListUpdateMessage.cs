using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Bots;

public record BotSkillListUpdateMessage : IComposer
{
    public int BotId { get; init; }
    public IList<BotSkillData> SkillList { get; init; }
}