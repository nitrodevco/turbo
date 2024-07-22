using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Catalog;
using Turbo.Core.Game.Catalog.Constants;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Catalog;
using Turbo.Packets.Outgoing.Catalog;

namespace Turbo.PacketHandlers;

public class CatalogMessageHandler : ICatalogMessageHandler
{
    private readonly ICatalogManager _catalogManager;
    private readonly ILogger<ICatalogMessageHandler> _logger;
    private readonly IPacketMessageHub _messageHub;

    public CatalogMessageHandler(
        IPacketMessageHub messageHub,
        ICatalogManager catalogManager,
        ILogger<ICatalogMessageHandler> logger)
    {
        _messageHub = messageHub;
        _catalogManager = catalogManager;
        _logger = logger;

        _messageHub.Subscribe<GetCatalogIndexMessage>(this, OnGetCatalogIndexMessage);
        _messageHub.Subscribe<GetCatalogPageMessage>(this, OnGetCatalogPageMessage);
        _messageHub.Subscribe<PurchaseFromCatalogMessage>(this, OnPurchaseFromCatalogMessage);
        _messageHub.Subscribe<GetProductOfferMessage>(this, OnGetProductOfferMessage);
    }

    public void OnGetCatalogIndexMessage(GetCatalogIndexMessage message, ISession session)
    {
        if (session.Player == null) return;

        var root = _catalogManager.GetRootForPlayer(session.Player, message.Type);

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

        var page = _catalogManager.GetPageForPlayer(session.Player, message.Type, message.PageId);

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

        _catalogManager.PurchaseOfferForPlayer(session.Player, CatalogType.Normal, message.PageId, message.OfferId,
            message.ExtraParam, message.Quantity);
    }

    public void OnGetProductOfferMessage(GetProductOfferMessage message, ISession session)
    {
        if (session.Player == null) return;

        var offer = _catalogManager.GetOfferForPlayer(session.Player, CatalogType.Normal, message.OfferId);

        if (offer == null) return;

        session.Send(new ProductOfferMessage
        {
            Offer = offer
        });
    }
}