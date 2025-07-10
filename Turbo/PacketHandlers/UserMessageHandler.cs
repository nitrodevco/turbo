using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Networking.Game.Clients;
using Turbo.Packets.Incoming.Preferences;
using Turbo.Packets.Incoming.Users;
using Turbo.Packets.Outgoing.Users;

namespace Turbo.Main.PacketHandlers;

public class UserMessageHandler(
    IPacketMessageHub messageHub,
    IPlayerManager playerManager) : IPacketHandlerManager
{
    public void Register()
    {
        messageHub.Subscribe<GetRelationshipStatusInfoMessage>(this, OnGetRelationshipStatusInfo);
        messageHub.Subscribe<GetSelectedBadgesMessage>(this, OnGetSelectedBadgesMessage);
        messageHub.Subscribe<ChatStylePreferenceMessage>(this, OnChatStylePreferenceMessage);
        messageHub.Subscribe<GetExtendedProfileMessage>(this, OnExtendedProfileMessage);
    }

    protected virtual async void OnGetRelationshipStatusInfo(GetRelationshipStatusInfoMessage message, ISession session)
    {
        if (session.Player == null) return;

        var player = playerManager.GetPlayerById(message.PlayerId) ?? await playerManager.GetOfflinePlayerById(message.PlayerId);

        if (player == null) return;

        await session.Send(new RelationshipStatusInfoMessage
        {
            Player = player
        });
    }

    protected virtual async void OnGetSelectedBadgesMessage(GetSelectedBadgesMessage message, ISession session)
    {
        if (session.Player == null) return;

        var activeBadges = await playerManager.GetPlayerActiveBadges(message.PlayerId);

        await session.Send(new UserBadgesMessage
        {
            PlayerId = message.PlayerId,
            ActiveBadges = activeBadges
        });
    }

    protected virtual void OnChatStylePreferenceMessage(ChatStylePreferenceMessage message, ISession session)
    {
        if (session.Player == null) return;

        session.Player.PlayerDetails?.SetPreferredChatStyleByClientId(message.StyleId);
    }

    protected virtual async Task OnExtendedProfileMessage(GetExtendedProfileMessage message, ISession session)
    {
        if (session.Player == null) return;

        var player = playerManager.GetPlayerById(message.PlayerId) ?? await playerManager.GetOfflinePlayerById(message.PlayerId);

        if (player == null) return;

        await session.Send(new ExtendedProfileMessage
        {
            Player = player
        });
    }

}