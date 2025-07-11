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

public class RoomEngineMessageHandler(
    IPacketMessageHub messageHub,
    INavigatorManager navigatorManager)
    : IPacketHandlerManager
{
    public void Register()
    {
        messageHub.Subscribe<ClickFurniMessage>(this, OnClickFurniMessage);
        messageHub.Subscribe<GetFurnitureAliasesMessage>(this, OnGetFurnitureAliasesMessage);
        messageHub.Subscribe<GetItemDataMessage>(this, OnGetItemDataMessage);
        messageHub.Subscribe<GetRoomEntryDataMessage>(this, OnGetRoomEntryDataMessage);
        messageHub.Subscribe<MoveAvatarMessage>(this, OnMoveAvatarMessage);
        messageHub.Subscribe<MoveObjectMessage>(this, OnMoveObjectMessage);
        messageHub.Subscribe<MoveWallItemMessage>(this, OnMoveWallItemMessage);
        messageHub.Subscribe<PickupObjectMessage>(this, OnPickupObjectMessage);
        messageHub.Subscribe<PlaceObjectMessage>(this, OnPlaceObjectMessage);
        messageHub.Subscribe<RemoveItemMessage>(this, OnRemoveItemMessage);
        messageHub.Subscribe<SetItemDataMessage>(this, OnSetItemDataMessage);
        messageHub.Subscribe<SetObjectDataMessage>(this, OnSetObjectDataMessage);
        messageHub.Subscribe<UseFurnitureMessage>(this, OnUseFurnitureMessage);
        messageHub.Subscribe<UseWallItemMessage>(this, OnUseWallItemMessage);
        messageHub.Subscribe<ChatMessage>(this, OnChatMessage);
        messageHub.Subscribe<WhisperMessage>(this, OnWhisperMessage);
        messageHub.Subscribe<ShoutMessage>(this, OnShoutMessage);
        messageHub.Subscribe<GetHeightMapMessage>(this, OnGetHeightMapMessage);
    }

    protected virtual void OnClickFurniMessage(ClickFurniMessage message, ISession session)
    {
        if (session.Player == null) return;

        //User clicks furni
    }

    protected virtual async void OnGetFurnitureAliasesMessage(GetFurnitureAliasesMessage message, ISession session)
    {
        if (session.Player == null) return;

        await session.SendQueue(new FurnitureAliasesMessage
        {
            Aliases = new Dictionary<string, string>()
        });
    }

    protected virtual void OnGetItemDataMessage(GetItemDataMessage message, ISession session)
    {
        if (session.Player == null) return;

        var room = session.Player.RoomObject?.Room;

        if (room == null) return;

        //TODO post it note / wall item data
    }

    protected virtual async void OnGetRoomEntryDataMessage(GetRoomEntryDataMessage message, ISession session)
    {
        //TODO WIN Version Doesn't need this
        if (session.Player == null) return;

        await navigatorManager.ContinueEnteringRoom(session.Player);
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
        if (session.Player?.RoomObject?.Room?.RoomFurnitureManager == null) return;

        var furnitureManager = session.Player.RoomObject.Room.RoomFurnitureManager;

        switch (message.ObjectCategory)
        {
            case 2:
            case 10:
                furnitureManager.RemoveFloorFurnitureByObjectId(session.Player, message.ObjectId);
                break;

            case 1:
            case 20:
                furnitureManager.RemoveWallFurnitureByObjectId(session.Player, message.ObjectId);
                break;
        }
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
    
    public virtual void OnGetHeightMapMessage(GetHeightMapMessage message, ISession session)
    {
        // Seems like this event no longer used on latest versions of Habbo.
        if (session.Player == null) return;

        //Send RoomEntryTile
        //Send HeightMap
        //Send FloorHeightMap
    }
}