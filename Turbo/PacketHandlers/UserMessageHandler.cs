using Turbo.Core.Game.Players;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Preferences;
using Turbo.Packets.Incoming.Users;
using Turbo.Packets.Outgoing.Users;

namespace Turbo.Main.PacketHandlers;

public class UserMessageHandler : IUserMessageHandler
{
    private readonly IPacketMessageHub _messageHub;
    private readonly IPlayerManager _playerManager;

    public UserMessageHandler(
        IPacketMessageHub messageHub,
        IPlayerManager playerManager)
    {
        _messageHub = messageHub;
        _playerManager = playerManager;

        _messageHub.Subscribe<GetSelectedBadgesMessage>(this, OnGetSelectedBadgesMessage);
        _messageHub.Subscribe<ChatStylePreferenceMessage>(this, OnChatStylePreferenceMessage);
    }

    protected virtual async void OnGetSelectedBadgesMessage(GetSelectedBadgesMessage message, ISession session)
    {
        if (session.Player == null) return;

        var activeBadges = await _playerManager.GetPlayerActiveBadges(message.PlayerId);

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