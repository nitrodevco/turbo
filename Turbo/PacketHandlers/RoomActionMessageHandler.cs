using System.Threading.Tasks;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Room.Action;

namespace Turbo.Main.PacketHandlers;

public class RoomActionMessageHandler(
    IPacketMessageHub messageHub) : IPacketHandlerManager
{
    public void Register()
    {
        messageHub.Subscribe<AmbassadorAlertMessage>(this, OnAmbassadorAlertMessage);
        messageHub.Subscribe<AssignRightsMessage>(this, OnAssignRightsMessage);
        messageHub.Subscribe<BanUserWithDurationMessage>(this, OnBanUserWithDurationMessage);
        messageHub.Subscribe<LetUserInMessage>(this, OnLetUserInMessage);
        messageHub.Subscribe<MuteAllInRoomMessage>(this, OnMuteAllInRoomMessage);
        messageHub.Subscribe<RemoveAllRightsMessage>(this, OnRemoveAllRightsMessage);
        messageHub.Subscribe<RemoveRightsMessage>(this, OnRemoveRightsMessage);
        messageHub.Subscribe<KickUserMessage>(this, OnRoomUserKickMessage);
        messageHub.Subscribe<MuteUserMessage>(this, OnRoomUserMuteMessage);
        messageHub.Subscribe<UnbanUserFromRoomMessage>(this, OnUnbanUserFromRoomMessage);
    }

    private void OnAmbassadorAlertMessage(AmbassadorAlertMessage message, ISession session)
    {
        if (session.Player == null) return;
    }

    private async Task OnAssignRightsMessage(AssignRightsMessage message, ISession session)
    {
        if (session.Player == null) return;

        await session.Player.RoomObject?.Room?.RoomSecurityManager?.AdjustRightsForPlayerId(session.Player,
            message.PlayerId, true);
    }

    private async Task OnBanUserWithDurationMessage(BanUserWithDurationMessage message, ISession session)
    {
        if (session.Player == null) return;

        var durationMs = 0.0;

        if (message.BanType.Equals("RWUAM_BAN_USER_HOUR")) durationMs = 3600000.0;
        if (message.BanType.Equals("RWUAM_BAN_USER_DAY")) durationMs = 86400000.0;
        if (message.BanType.Equals("RWUAM_BAN_USER_PERM")) durationMs = 15768000000.0; // 5 years

        await session.Player.RoomObject?.Room?.RoomSecurityManager?.BanPlayerIdWithDuration(session.Player,
            message.PlayerId, durationMs);
    }

    private void OnLetUserInMessage(LetUserInMessage message, ISession session)
    {
        if (session.Player == null) return;
    }

    private void OnMuteAllInRoomMessage(MuteAllInRoomMessage message, ISession session)
    {
        if (session.Player == null) return;
    }

    private async Task OnRemoveAllRightsMessage(RemoveAllRightsMessage message, ISession session)
    {
        await session.Player.RoomObject?.Room?.RoomSecurityManager?.RemoveAllRights(session.Player);
    }

    private async Task OnRemoveRightsMessage(RemoveRightsMessage message, ISession session)
    {
        if (session.Player == null) return;

        var roomSecurityManager = session.Player.RoomObject?.Room?.RoomSecurityManager;

        if (roomSecurityManager == null) return;

        foreach (var playerId in message.PlayerIds)
            await roomSecurityManager.AdjustRightsForPlayerId(session.Player, playerId, false);
    }

    private void OnRoomUserKickMessage(KickUserMessage message, ISession session)
    {
        session.Player.RoomObject?.Room?.RoomSecurityManager?.KickPlayer(session.Player, message.PlayerId);
    }

    private void OnRoomUserMuteMessage(MuteUserMessage message, ISession session)
    {
        if (session.Player == null) return;
    }

    private void OnUnbanUserFromRoomMessage(UnbanUserFromRoomMessage message, ISession session)
    {
        if (session.Player == null) return;
    }
}