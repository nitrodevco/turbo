using Turbo.Core.Game.Rooms;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Inventory.Badges;
using Turbo.Packets.Incoming.Inventory.Furni;

namespace Turbo.Main.PacketHandlers;

public class InventoryMessageHandler : IInventoryMessageHandler
{
    private readonly IPacketMessageHub _messageHub;
    private readonly IRoomManager _roomManager;

    public InventoryMessageHandler(
        IPacketMessageHub messageHub,
        IRoomManager roomManager)
    {
        _messageHub = messageHub;
        _roomManager = roomManager;

        _messageHub.Subscribe<GetBadgesMessage>(this, OnGetBadgesMessage);
        _messageHub.Subscribe<SetActivatedBadgesMessage>(this, OnSetActivatedBadgesMessage);
        _messageHub.Subscribe<RequestFurniInventoryMessage>(this, OnRequestFurniInventoryMessage);
        _messageHub.Subscribe<RequestFurniInventoryWhenNotInRoomMessage>(this,
            OnRequestFurniInventoryWhenNotInRoomMessage);
        _messageHub.Subscribe<RequestRoomPropertySetMessage>(this, OnRequestRoomPropertySetMessage);
    }

    protected virtual void OnGetBadgesMessage(GetBadgesMessage message, ISession session)
    {
        if (session.Player == null) return;

        var playerBadgeInventory = session.Player.PlayerInventory.BadgeInventory;

        if (playerBadgeInventory != null) playerBadgeInventory.SendBadgesToSession(session);
    }

    protected virtual void OnSetActivatedBadgesMessage(SetActivatedBadgesMessage message, ISession session)
    {
        if (session.Player == null) return;

        var playerBadgeInventory = session.Player.PlayerInventory.BadgeInventory;

        if (playerBadgeInventory != null) playerBadgeInventory.SetActivedBadges(message.Badges);
    }

    protected virtual void OnRequestFurniInventoryMessage(RequestFurniInventoryMessage message, ISession session)
    {
        if (session.Player == null) return;

        var playerFurnitureInventory = session.Player.PlayerInventory.FurnitureInventory;

        if (playerFurnitureInventory != null) playerFurnitureInventory.SendFurnitureToSession(session);
    }

    protected virtual void OnRequestFurniInventoryWhenNotInRoomMessage(
        RequestFurniInventoryWhenNotInRoomMessage message, ISession session)
    {
    }

    protected virtual void OnRequestRoomPropertySetMessage(RequestRoomPropertySetMessage message, ISession session)
    {
    }
}