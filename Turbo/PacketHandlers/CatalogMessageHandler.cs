using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Catalog;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Catalog;
using Turbo.Packets.Outgoing.Catalog;

namespace Turbo.PacketHandlers
{
    public class CatalogMessageHandler : ICatalogMessageHandler
    {
        private readonly IPacketMessageHub _messageHub;
        private readonly ICatalogManager _catalogManager;
        private readonly ILogger<ICatalogMessageHandler> _logger;

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
        }

        public void OnGetCatalogIndexMessage(GetCatalogIndexMessage message, ISession session)
        {
            if (session.Player == null) return;

            if (!_catalogManager.Catalogs.ContainsKey(message.Type)) return;

            var root = _catalogManager.Catalogs[message.Type].GetRootForPlayer(session.Player);

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

            if (!_catalogManager.Catalogs.ContainsKey(message.Type)) return;

            var page = _catalogManager.Catalogs[message.Type].GetPageForPlayer(session.Player, message.PageId, message.OfferId);

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
    }
}