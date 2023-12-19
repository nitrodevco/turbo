using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Turbo.Catalog.Factories;
using Turbo.Core.Game.Catalog;
using Turbo.Core.Game.Catalog.Constants;
using Turbo.Core.Game.Players;
using Turbo.Core.Utilities;
using Turbo.Database.Entities.Catalog;
using Turbo.Database.Repositories.Catalog;
using Turbo.Packets.Outgoing.Catalog;

namespace Turbo.Catalog
{
    public class CatalogManager : Component, ICatalogManager
    {
        private readonly ILogger<ICatalogManager> _logger;
        private readonly ICatalogFactory _catalogFactory;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public IDictionary<string, ICatalog> Catalogs { get; private set; }

        public CatalogManager(
            ILogger<ICatalogManager> logger,
            ICatalogFactory catalogFactory,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _catalogFactory = catalogFactory;
            _serviceScopeFactory = scopeFactory;

            Catalogs = new Dictionary<string, ICatalog>();
        }

        protected override async Task OnInit()
        {
            var normalCatalog = _catalogFactory.CreateCatalog(CatalogType.Normal);

            await normalCatalog.InitAsync();

            Catalogs.Add(normalCatalog.CatalogType, normalCatalog);
        }

        protected override async Task OnDispose()
        {

        }

        public ICatalogPage GetRootForPlayer(IPlayer player, string catalogType)
        {
            if (catalogType == null || catalogType == null) return null;

            if(Catalogs.TryGetValue(catalogType, out var catalog))
            {
                return catalog?.GetRootForPlayer(player) ?? null;
            }

            return null;
        }

        public async Task<bool> PurchaseOfferForPlayer(IPlayer player, string catalogType, int pageId, int offerId, string extraParam, int quantity)
        {
            if (player == null || catalogType == null) return false;

            if(Catalogs.TryGetValue(catalogType, out var catalog))
            {
                var purchasedOffer = await catalog.PurchaseOffer(player, pageId, offerId, extraParam, quantity);

                if(purchasedOffer == null)
                {
                    player.Session?.Send(new PurchaseNotAllowedMessage
                    {
                        ErrorCode = PurchaseNotAllowedEnum.Unknown
                    });

                    return false;
                }
                
                player.Session?.Send(new PurchaseOkMessage
                {
                    Offer = purchasedOffer
                });

                return true;
            }

            return false;
        }

        public ICatalogOffer GetOfferForPlayer(IPlayer player, string catalogType, int offerId)
        {
            if (player == null || catalogType == null) return null;

            if (Catalogs.TryGetValue(catalogType, out var catalog))
            {
                return catalog.GetOfferForPlayer(player, offerId) ?? null;
            }

            return null;
        }

        public ICatalogPage GetPageForPlayer(IPlayer player, string catalogType, int pageId)
        {
            if (player == null || catalogType == null) return null;

            if (Catalogs.TryGetValue(catalogType, out var catalog))
            {
                return catalog.GetPageForPlayer(player, pageId) ?? null;
            }

            return null;
        }
    }
}
