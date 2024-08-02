using System.Collections.Generic;
using System.Linq;
using Turbo.Core.Game.Catalog;
using Turbo.Core.Game.Catalog.Constants;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Catalog;
using Turbo.Packets.Outgoing.Catalog;

namespace Turbo.Main.PacketHandlers;

public class CatalogMessageHandler(
    IPacketMessageHub messageHub,
    ICatalogManager catalogManager)
    : IPacketHandlerManager
{
    public void Register()
    {
        messageHub.Subscribe<GetCatalogIndexMessage>(this, OnGetCatalogIndexMessage);
        messageHub.Subscribe<GetCatalogPageMessage>(this, OnGetCatalogPageMessage);
        messageHub.Subscribe<PurchaseFromCatalogMessage>(this, OnPurchaseFromCatalogMessage);
        messageHub.Subscribe<GetProductOfferMessage>(this, OnGetProductOfferMessage);
    }

    public void OnGetCatalogIndexMessage(GetCatalogIndexMessage message, ISession session)
    {
        if (session.Player == null) return;

        var root = catalogManager.GetRootForPlayer(session.Player, message.Type);

        if (root == null) return;

        session.Send(new CatalogIndexMessage
        {
            Root = root,
            NewAdditionsAvailable = false,
            CatalogType = message.Type
        });
    }

    public void OnGetCatalogPageMessage(GetCatalogPageMessage message, ISession session)
    {
        if (session.Player == null) return;

        var page = catalogManager.GetPageForPlayer(session.Player, message.Type, message.PageId);

        if (page == null) return;

        session.Send(new CatalogPageMessage
        {
            PageId = page.Id,
            CatalogType = message.Type,
            LayoutCode = page.Layout,
            ImageDatas = page.ImageDatas,
            TextDatas = page.TextDatas,
            Offers = page.Offers.Values.ToList(),
            OfferId = message.OfferId,
            AcceptSeasonCurrencyAsCredits = false,
            FrontPageItems = new List<ICatalogFrontPageItem>()
        });
    }

    public void OnPurchaseFromCatalogMessage(PurchaseFromCatalogMessage message, ISession session)
    {
        if (session.Player == null) return;

        catalogManager.PurchaseOfferForPlayer(session.Player, CatalogType.Normal, message.PageId, message.OfferId,
            message.ExtraParam, message.Quantity);
    }

    public void OnGetProductOfferMessage(GetProductOfferMessage message, ISession session)
    {
        if (session.Player == null) return;

        var offer = catalogManager.GetOfferForPlayer(session.Player, CatalogType.Normal, message.OfferId);

        if (offer == null) return;

        session.Send(new ProductOfferMessage
        {
            Offer = offer
        });
    }
}