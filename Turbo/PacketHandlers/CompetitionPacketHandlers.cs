using System;
using System.Linq;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Competition;
using Turbo.Packets.Outgoing.Competition;

namespace Turbo.Main.PacketHandlers;

public class CompetitionPacketHandlers(
    IPacketMessageHub messageHub
) : IPacketHandlerManager
{
    public void Register()
    {
        messageHub.Subscribe<GetCurrentTimingCodeMessage>(this, OnCurrentTimingCodeMessage);
    }

    private async void OnCurrentTimingCodeMessage(GetCurrentTimingCodeMessage msg, ISession session)
    {
        var timingCode = msg.TimingCode;
        var latestCompetition = string.Empty;

        if (!string.IsNullOrEmpty(timingCode))
        {
            var timingCodeSplit = timingCode.Split(';');
            var timingCodeOrdered = timingCodeSplit.OrderBy(x => DateTime.Parse(x.Split(',')[0]));
            latestCompetition = timingCodeOrdered.Last().Split(',')[1];
        }

        await session.Send(new CurrentTimingCodeMessage
        {
            SchedulingStr = timingCode,
            Code = latestCompetition
        });
    }
}