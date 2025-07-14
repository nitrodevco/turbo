using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Inventory.Achievements;
using Turbo.Packets.Incoming.Inventory.Badges;
using Turbo.Packets.Incoming.Inventory.Furni;
using Turbo.Packets.Incoming.Inventory.Purse;
using Turbo.Packets.Outgoing.Inventory.Achievements;
using Turbo.Packets.Outgoing.Inventory.Purse;

namespace Turbo.Main.PacketHandlers;

public class InventoryMessageHandler(
    IPacketMessageHub messageHub
) : IPacketHandlerManager
{
    public void Register()
    {
        messageHub.Subscribe<GetCreditsInfoMessage>(this, OnGetCreditsInfoMessage);
        messageHub.Subscribe<GetBadgesMessage>(this, OnGetBadgesMessage);
        messageHub.Subscribe<SetActivatedBadgesMessage>(this, OnSetActivatedBadgesMessage);
        messageHub.Subscribe<RequestFurniInventoryMessage>(this, OnRequestFurniInventoryMessage);
        messageHub.Subscribe<RequestFurniInventoryWhenNotInRoomMessage>(this, OnRequestFurniInventoryWhenNotInRoomMessage);
        messageHub.Subscribe<RequestRoomPropertySetMessage>(this, OnRequestRoomPropertySetMessage);
        messageHub.Subscribe<GetAchievementsMessage>(this, OnGetAchievementsMessage);
    }

    protected virtual void OnGetCreditsInfoMessage(GetCreditsInfoMessage message, ISession session)
    {
        if (session.Player == null) return;

        session.Send(new CreditBalanceMessage
        {
            credits = 100
        });
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

    protected virtual void OnRequestFurniInventoryWhenNotInRoomMessage(RequestFurniInventoryWhenNotInRoomMessage message, ISession session)
    {
        if (session.Player == null) return;

        var playerFurnitureInventory = session.Player.PlayerInventory.FurnitureInventory;

        if (playerFurnitureInventory != null) playerFurnitureInventory.SendFurnitureToSession(session);
    }

    protected virtual void OnRequestRoomPropertySetMessage(RequestRoomPropertySetMessage message, ISession session)
    {
    }

    private void OnGetAchievementsMessage(GetAchievementsMessage message, ISession session)
    {
        if (session.Player == null) return;

        session.Send(new AchievementsMessage());
    }
}