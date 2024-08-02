using System.Collections.Generic;
using Turbo.Core.Game.Navigator;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Constants;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Navigator;
using Turbo.Packets.Incoming.RoomSettings;
using Turbo.Packets.Outgoing.RoomSettings;

namespace Turbo.Main.PacketHandlers;

public class RoomSettingsMessageHandler(
    IPacketMessageHub messageHub,
    IPlayerManager playerManager,
    IRoomManager roomManager,
    INavigatorManager navigatorManager)
    : IPacketHandlerManager
{
    private readonly INavigatorManager _navigatorManager = navigatorManager;

    public void Register()
    {
        messageHub.Subscribe<DeleteRoomMessage>(this, OnDeleteRoomMessage);
        messageHub.Subscribe<GetBannedUsersFromRoomMessage>(this, OnGetBannedUsersFromRoomMessage);
        messageHub.Subscribe<GetCustomRoomFilterMessage>(this, OnGetCustomRoomFilterMessage);
        messageHub.Subscribe<GetFlatControllersMessage>(this, OnGetFlatControllersMessage);
        messageHub.Subscribe<GetRoomSettingsMessage>(this, OnGetRoomSettingsMessage);
        messageHub.Subscribe<SaveRoomSettingsMessage>(this, OnSaveRoomSettingsMessage);
        messageHub.Subscribe<UpdateRoomCategoryAndTradeSettingsMessage>(this,
            OnUpdateRoomCategoryAndTradeSettingsMessage);
        messageHub.Subscribe<UpdateRoomFilterMessage>(this, OnUpdateRoomFilterMessage);
    }

    protected virtual void OnDeleteRoomMessage(DeleteRoomMessage message, ISession session)
    {
        if (session.Player == null) return;
    }

    protected virtual async void OnGetBannedUsersFromRoomMessage(GetBannedUsersFromRoomMessage message,
        ISession session)
    {
        if (session.Player == null) return;

        var room = await roomManager.GetOfflineRoom(message.RoomId);

        if (room == null) return;

        await room.RoomSecurityManager.InitAsync();

        if (!room.RoomSecurityManager.IsOwner(session.Player)) return;

        Dictionary<int, string> bans = new();

        foreach (var playerId in room.RoomSecurityManager.Bans.Keys)
        {
            var player = playerManager.GetPlayerById(playerId);

            if (player != null)
            {
                bans.Add(player.Id, player.Name);
            }
            else
            {
                var username = await playerManager.GetPlayerName(playerId);

                bans.Add(playerId, username);
            }
        }

        await session.Send(new BannedUsersFromRoomMessage
        {
            RoomId = room.Id,
            Players = bans
        });
    }

    protected virtual void OnGetCustomRoomFilterMessage(GetCustomRoomFilterMessage message, ISession session)
    {
        if (session.Player == null) return;
    }

    protected virtual async void OnGetFlatControllersMessage(GetFlatControllersMessage message, ISession session)
    {
        if (session.Player == null) return;

        var room = await roomManager.GetOfflineRoom(message.RoomId);

        if (room == null) return;

        await room.RoomSecurityManager.InitAsync();

        if (!room.RoomSecurityManager.IsOwner(session.Player)) return;

        Dictionary<int, string> controllers = new();

        foreach (var playerId in room.RoomSecurityManager.Rights)
        {
            var player = playerManager.GetPlayerById(playerId);

            if (player != null)
            {
                controllers.Add(player.Id, player.Name);
            }
            else
            {
                var username = await playerManager.GetPlayerName(playerId);

                controllers.Add(playerId, username);
            }
        }

        await session.Send(new FlatControllersMessage
        {
            RoomId = room.Id,
            Players = controllers
        });
    }

    protected virtual async void OnGetRoomSettingsMessage(GetRoomSettingsMessage message, ISession session)
    {
        if (session.Player == null) return;

        var room = await roomManager.GetOfflineRoom(message.RoomId);

        if (room == null)
        {
            await session.Send(new RoomSettingsErrorMessage
            {
                RoomId = message.RoomId,
                ErrorCode = RoomSettingsErrorType.RoomNotFound
            });

            return;
        }

        await room.RoomSecurityManager.InitAsync();

        if (!room.RoomSecurityManager.IsOwner(session.Player))
        {
            await session.Send(new RoomSettingsErrorMessage
            {
                RoomId = message.RoomId,
                ErrorCode = RoomSettingsErrorType.NotOwner
            });

            return;
        }

        await session.Send(new RoomSettingsDataMessage
        {
            RoomDetails = room.RoomDetails
        });
    }

    protected virtual async void OnSaveRoomSettingsMessage(SaveRoomSettingsMessage message, ISession session)
    {
        if (session.Player == null) return;

        var room = await roomManager.GetOfflineRoom(message.RoomId);

        if (room == null) return;

        await room.RoomSecurityManager.InitAsync();

        if (!room.RoomSecurityManager.IsOwner(session.Player))
        {
            await session.Send(new RoomSettingsErrorMessage
            {
                RoomId = message.RoomId,
                ErrorCode = RoomSettingsErrorType.NotOwner
            });

            return;
        }

        if (!room.RoomDetails.UpdateSettingsForPlayer(session.Player, message)) return;

        await session.Send(new RoomSettingsSavedMessage
        {
            RoomId = room.Id
        });
    }

    protected virtual void OnUpdateRoomCategoryAndTradeSettingsMessage(
        UpdateRoomCategoryAndTradeSettingsMessage message, ISession session)
    {
        if (session.Player == null) return;
    }

    protected virtual void OnUpdateRoomFilterMessage(UpdateRoomFilterMessage message, ISession session)
    {
        if (session.Player == null) return;
    }
}