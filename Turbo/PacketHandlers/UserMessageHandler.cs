using Turbo.Core.Game.Players;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Preferences;
using Turbo.Packets.Incoming.Users;
using Turbo.Packets.Outgoing.Users;

namespace Turbo.Main.PacketHandlers;

public class UserMessageHandler(
    IPacketMessageHub messageHub,
    IPlayerManager playerManager)
    : IPacketHandlerManager
{
    public void Register()
    {
        messageHub.Subscribe<GetSelectedBadgesMessage>(this, OnGetSelectedBadgesMessage);
        messageHub.Subscribe<ChatStylePreferenceMessage>(this, OnChatStylePreferenceMessage);
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
}