using System.Threading.Tasks;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Room.Action;

namespace Turbo.PacketHandlers;

public class RoomActionMessageHandler : IRoomActionMessageHandler
{
    private readonly IPacketMessageHub _messageHub;
    private readonly IRoomManager _roomManager;

    public RoomActionMessageHandler(
        IPacketMessageHub messageHub,
        IRoomManager roomManager)
    {
        _messageHub = messageHub;
        _roomManager = roomManager;

        _messageHub.Subscribe<AmbassadorAlertMessage>(this, OnAmbassadorAlertMessage);
        _messageHub.Subscribe<AssignRightsMessage>(this, OnAssignRightsMessage);
        _messageHub.Subscribe<BanUserWithDurationMessage>(this, OnBanUserWithDurationMessage);
        _messageHub.Subscribe<LetUserInMessage>(this, OnLetUserInMessage);
        _messageHub.Subscribe<MuteAllInRoomMessage>(this, OnMuteAllInRoomMessage);
        _messageHub.Subscribe<RemoveAllRightsMessage>(this, OnRemoveAllRightsMessage);
        _messageHub.Subscribe<RemoveRightsMessage>(this, OnRemoveRightsMessage);
        _messageHub.Subscribe<KickUserMessage>(this, OnRoomUserKickMessage);
        _messageHub.Subscribe<MuteUserMessage>(this, OnRoomUserMuteMessage);
        _messageHub.Subscribe<UnbanUserFromRoomMessage>(this, OnUnbanUserFromRoomMessage);
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