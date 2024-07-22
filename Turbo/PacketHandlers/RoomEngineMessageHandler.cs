using System.Collections.Generic;
using Turbo.Core.Game.Navigator;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Room.Chat;
using Turbo.Packets.Incoming.Room.Engine;
using Turbo.Packets.Outgoing.Room.Engine;
using Turbo.Rooms.Utils;

namespace Turbo.Main.PacketHandlers;

public class RoomEngineMessageHandler : IRoomEngineMessageHandler
{
    private readonly IPacketMessageHub _messageHub;
    private readonly INavigatorManager _navigatorManager;
    private readonly IRoomManager _roomManager;

    public RoomEngineMessageHandler(
        IPacketMessageHub messageHub,
        IRoomManager roomManager,
        INavigatorManager navigatorManager)
    {
        _messageHub = messageHub;
        _roomManager = roomManager;
        _navigatorManager = navigatorManager;

        _messageHub.Subscribe<GetFurnitureAliasesMessage>(this, OnGetFurnitureAliasesMessage);
        _messageHub.Subscribe<GetItemDataMessage>(this, OnGetItemDataMessage);
        _messageHub.Subscribe<GetRoomEntryDataMessage>(this, OnGetRoomEntryDataMessage);
        _messageHub.Subscribe<MoveAvatarMessage>(this, OnMoveAvatarMessage);
        _messageHub.Subscribe<MoveObjectMessage>(this, OnMoveObjectMessage);
        _messageHub.Subscribe<MoveWallItemMessage>(this, OnMoveWallItemMessage);
        _messageHub.Subscribe<PickupObjectMessage>(this, OnPickupObjectMessage);
        _messageHub.Subscribe<PlaceObjectMessage>(this, OnPlaceObjectMessage);
        _messageHub.Subscribe<RemoveItemMessage>(this, OnRemoveItemMessage);
        _messageHub.Subscribe<SetItemDataMessage>(this, OnSetItemDataMessage);
        _messageHub.Subscribe<SetObjectDataMessage>(this, OnSetObjectDataMessage);
        _messageHub.Subscribe<UseFurnitureMessage>(this, OnUseFurnitureMessage);
        _messageHub.Subscribe<UseWallItemMessage>(this, OnUseWallItemMessage);
        _messageHub.Subscribe<ChatMessage>(this, OnChatMessage);
        _messageHub.Subscribe<WhisperMessage>(this, OnWhisperMessage);
        _messageHub.Subscribe<ShoutMessage>(this, OnShoutMessage);
    }

    protected virtual async void OnGetFurnitureAliasesMessage(GetFurnitureAliasesMessage message, ISession session)
    {
        if (session.Player == null) return;

        await session.Send(new FurnitureAliasesMessage
        {
            Aliases = new Dictionary<string, string>()
        });
    }

    protected virtual void OnGetItemDataMessage(GetItemDataMessage message, ISession session)
    {
        if (session.Player == null) return;

        var room = session.Player.RoomObject?.Room;

        if (room == null) return;

        // post it note / wall item data
    }

    protected virtual async void OnGetRoomEntryDataMessage(GetRoomEntryDataMessage message, ISession session)
    {
        if (session.Player == null) return;

        await _navigatorManager.ContinueEnteringRoom(session.Player);
    }

    protected virtual void OnMoveAvatarMessage(MoveAvatarMessage message, ISession session)
    {
        if (session.Player == null) return;

        session.Player.RoomObject?.Logic?.WalkTo(new Point(message.X, message.Y), true);
    }

    protected virtual void OnMoveObjectMessage(MoveObjectMessage message, ISession session)
    {
        if (session.Player == null) return;

        session.Player.RoomObject?.Room?.RoomFurnitureManager?.MoveFloorFurniture(session.Player, message.ObjectId,
            message.X, message.Y, (Rotation)message.Direction);
    }

    protected virtual void OnMoveWallItemMessage(MoveWallItemMessage message, ISession session)
    {
        if (session.Player == null) return;

        session.Player.RoomObject?.Room?.RoomFurnitureManager?.MoveWallFurniture(session.Player, message.ObjectId,
            message.Location);
    }

    protected virtual void OnPickupObjectMessage(PickupObjectMessage message, ISession session)
    {
        if (session.Player == null) return;

        if (message.ObjectCategory == 10)
        {
            session.Player.RoomObject?.Room?.RoomFurnitureManager?.RemoveFloorFurnitureByObjectId(session.Player,
                message.ObjectId);

            return;
        }

        if (message.ObjectCategory == 20)
            session.Player.RoomObject?.Room?.RoomFurnitureManager?.RemoveWallFurnitureByObjectId(session.Player,
                message.ObjectId);
    }

    protected virtual void OnPlaceObjectMessage(PlaceObjectMessage message, ISession session)
    {
        if (session.Player == null) return;

        if (message.WallLocation == null)
            session.Player.RoomObject?.Room?.RoomFurnitureManager?.PlaceFloorFurnitureByFurniId(session.Player,
                message.ObjectId, new Point(message.X ?? 0, message.Y ?? 0, 0, (Rotation)message.Direction));

        if (message.WallLocation != null && message.WallLocation.Length > 0)
            session.Player.RoomObject?.Room?.RoomFurnitureManager?.PlaceWallFurnitureByFurniId(session.Player,
                message.ObjectId, message.WallLocation);
    }

    protected virtual void OnRemoveItemMessage(RemoveItemMessage message, ISession session)
    {
        if (session.Player == null) return;

        IRoomObject roomObject = session.Player.RoomObject;

        if (roomObject == null) return;

        // delete sticky
    }

    protected virtual void OnSetItemDataMessage(SetItemDataMessage message, ISession session)
    {
        if (session.Player == null) return;

        // save sticky note
    }

    protected virtual void OnSetObjectDataMessage(SetObjectDataMessage message, ISession session)
    {
        if (session.Player == null) return;

        // save strings of data, room background
    }

    protected virtual void OnUseFurnitureMessage(UseFurnitureMessage message, ISession session)
    {
        if (session.Player == null) return;

        session.Player.RoomObject?.Room?.RoomFurnitureManager?.FloorObjects?.GetRoomObject(message.ObjectId)?.Logic
            ?.OnInteract(session.Player.RoomObject, message.Param);
    }

    protected virtual void OnUseWallItemMessage(UseWallItemMessage message, ISession session)
    {
        if (session.Player == null) return;

        session.Player.RoomObject?.Room?.RoomFurnitureManager?.WallObjects?.GetRoomObject(message.ObjectId)?.Logic
            ?.OnInteract(session.Player.RoomObject, message.Param);
    }

    protected virtual void OnChatMessage(ChatMessage message, ISession session)
    {
        if (session.Player == null) return;

        var roomObject = session.Player.RoomObject;

        if (roomObject == null) return;

        roomObject.Room?.RoomChatManager?.SendChatForPlayer(session.Player, message.Text, message.StyleId);
    }

    protected virtual void OnWhisperMessage(WhisperMessage message, ISession session)
    {
        if (session.Player == null) return;

        var roomObject = session.Player.RoomObject;

        if (roomObject == null) return;

        roomObject.Room?.RoomChatManager?.SendWhisperForPlayer(session.Player, message.RecipientName, message.Text,
            message.StyleId);
    }

    protected virtual void OnShoutMessage(ShoutMessage message, ISession session)
    {
        if (session.Player == null) return;

        var roomObject = session.Player.RoomObject;

        if (roomObject == null) return;

        roomObject.Room?.RoomChatManager?.SendShoutForPlayer(session.Player, message.Text, message.StyleId);
    }
}