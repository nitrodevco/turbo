using System;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Game.Messenger.Constants;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Players.Constants;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.FriendList;
using Turbo.Packets.Outgoing.FriendList;
using Turbo.Players;
using FriendListUpdateEventMessage = Turbo.Packets.Incoming.FriendList.FriendListUpdateMessage;
using FriendListUpdateMessage = Turbo.Packets.Outgoing.FriendList.FriendListUpdateMessage;
using MessengerInitEventMessage = Turbo.Packets.Incoming.FriendList.MessengerInitMessage;
using MessengerInitMessage = Turbo.Packets.Outgoing.FriendList.MessengerInitMessage;

namespace Turbo.Main.PacketHandlers;

public class FriendListMessageHandler(
    IPacketMessageHub messageHub,
    IPlayerManager _playerManager) : IPacketHandlerManager
{
    public void Register()
    {
        messageHub.Subscribe<AcceptFriendMessage>(this, OnAcceptFriendMessage);
        messageHub.Subscribe<DeclineFriendMessage>(this, OnDeclineRequestMessage);
        messageHub.Subscribe<FindNewFriendsMessage>(this, OnFindNewFriendMessage);
        messageHub.Subscribe<FollowFriendMessage>(this, OnFollowFriendMessage);
        messageHub.Subscribe<FriendListUpdateEventMessage>(this, OnFriendListUpdateMessage);
        messageHub.Subscribe<GetFriendRequestsMessage>(this, OnGetFriendRequestsMessage);
        messageHub.Subscribe<GetMessengerHistoryMessage>(this, OnGetMessengerHistoryMessage);
        messageHub.Subscribe<HabboSearchMessage>(this, OnHabboSearchMessage);
        messageHub.Subscribe<MessengerInitEventMessage>(this, OnMessengerInitMessage);
        messageHub.Subscribe<RemoveFriendMessage>(this, OnRemoveFriendMessage);
        messageHub.Subscribe<RequestFriendMessage>(this, OnRequestFriendMessage);
        messageHub.Subscribe<SendMsgMessage>(this, OnSendMsgMessage);
        messageHub.Subscribe<SendRoomInviteMessage>(this, OnSendRoomInviteMessage);
        messageHub.Subscribe<VisitUserMessage>(this, OnVisitUserMessage);
    }

    private async Task OnAcceptFriendMessage(AcceptFriendMessage message, ISession session)
    {
        if (session.Player == null || message.Friends == null || message.Friends.Count == 0)
            return;

        var messenger = session.Player.Messenger;

        await session.Player.Messenger.AcceptFriends(message.Friends.ToArray());
    }

    private async void OnDeclineRequestMessage(DeclineFriendMessage message, ISession session)
    {
        if (session.Player == null || !session.Player.IsInitialized)
            return;

        var messenger = session.Player.Messenger;

        if (message.DeclineAll)
        {
            await messenger.ClearRequests();
        }
        else
        {
            if (message.Friends is null || message.Friends.Count == 0)
                return;

            await messenger.DeleteRequests(message.Friends.ToArray());
        }
    }

    private void OnFollowFriendMessage(FollowFriendMessage message, ISession session)
    {
        if (session.Player == null) return;

        if (message.PlayerId <= 0)
            return;
    }

    private void OnFindNewFriendMessage(FindNewFriendsMessage message, ISession session) => throw new NotImplementedException();

    private void OnFriendListUpdateMessage(FriendListUpdateEventMessage message, ISession session)
    {
        if (session.Player == null || !session.Player.IsInitialized) return;

        var messenger = session.Player.Messenger;

        session.Send(new FriendListUpdateMessage
        {
            FriendListUpdate = messenger.GetAndClearUpdates()
        });
    }

    private void OnGetFriendRequestsMessage(GetFriendRequestsMessage message, ISession session)
    {
        if (session.Player == null || !session.Player.IsInitialized) return;

        var messenger = session.Player.Messenger;

        session.Send(new FriendRequestsMessage
        {
            Requests = messenger.Requests.Values.ToList()
        });
    }

    private void OnGetMessengerHistoryMessage(GetMessengerHistoryMessage message, ISession session) => throw new NotImplementedException();

    private void OnHabboSearchMessage(HabboSearchMessage message, ISession session)
    {
        if (session.Player == null || String.IsNullOrWhiteSpace(message.SearchQuery))
            return;


    }

    private void OnMessengerInitMessage(MessengerInitEventMessage message, ISession session)
    {
        if (session.Player == null || !session.Player.IsInitialized) return;

        session.Send(new MessengerInitMessage
        {
            userFriendLimit = 500,
            normalFriendLimit = 500,
            extendedFriendLimit = 3000
            //Missing categories, not implemented yet
        });

        var messenger = session.Player.Messenger;

        session.Send(new FriendListFragmentMessage
        {
            FriendListFragments = messenger.GetFriendsFragments(100)
        });
    }

    private async Task OnRemoveFriendMessage(RemoveFriendMessage message, ISession session)
    {
        if (session.Player is null || message.FriendIds is null || message.FriendIds.Count == 0) return;

        var messenger = session.Player.Messenger;

        await messenger.RemoveFriends(message.FriendIds.ToArray());
    }

    private async Task OnRequestFriendMessage(RequestFriendMessage message, ISession session)
    {
        if (session.Player == null || string.IsNullOrWhiteSpace(message.PlayerName))
            return;

        var messenger = session.Player.Messenger;

        var targetPlayer = await _playerManager.GetOfflinePlayerByUsername(message.PlayerName);

        if (targetPlayer == null)
        {
            await session.Send(new MessengerErrorMessage
            {
                ErrorCode = MessengerErrorEnum.TargetNotFound
            });
            return;
        }

        var targetMessenger = targetPlayer.Messenger;

        //if (targetPlayer.PlayerPreferences.isBlockingFriendRequests)
        //{
        //    await session.Send(new MessengerErrorMessage
        //    {
        //        ErrorCode = MessengerErrorEnum.TargetBlockingRequests
        //    });
        //    return;
        //}

        const int maxFriends = 500;

        if (messenger.GetFriendsCount() >= maxFriends)
        {
            await session.Send(new MessengerErrorMessage
            {
                ErrorCode = MessengerErrorEnum.FriendListFull
            });
            return;
        }

        if (targetMessenger.GetFriendsCount() >= maxFriends)
        {
            await session.Send(new MessengerErrorMessage
            {
                ErrorCode = MessengerErrorEnum.TargetFriendListFull
            });
            return;
        }

        var request = await messenger.SendRequest(targetPlayer);

        if (request is null) return;

        if (targetPlayer.PlayerDetails.PlayerStatus is PlayerStatusEnum.Online)
        {
            targetPlayer.Messenger.AddRequest(request);
            
            await targetPlayer.Session.Send(new NewFriendRequestMessage
            {
                Request = request
            });
        }
    }

    private void OnSendMsgMessage(SendMsgMessage message, ISession session)
    {
        if (session.Player == null || message.ChatId <= 0 || string.IsNullOrWhiteSpace(message.Message))
            return;

        throw new NotImplementedException();
        //Find the friend by ID
        //If Friend does not exist, skip
        //Send the message to the friend
        //If friend is offline, store the message for later delivery
    }

    private void OnSendRoomInviteMessage(SendRoomInviteMessage message, ISession session)
    {
        if (session.Player == null) return;

        if (String.IsNullOrWhiteSpace(message.Message)) return;

        if (message.FriendIds == null || message.FriendIds.Count == 0)
            return;

        throw new NotImplementedException();

        //foreach (var friendId in message.FriendIds)
        //{
        //    //Find the friend by ID
        //    //If Friend does not exist, skip

        //    //Messenger Sent Invite even if offline, he will receive it when he logs in
        //}
    }

    private void OnVisitUserMessage(VisitUserMessage message, ISession session) => throw new NotImplementedException();
}
