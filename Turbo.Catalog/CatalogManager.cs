using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Turbo.Catalog.Factories;
using Turbo.Core.Game.Catalog;
using Turbo.Core.Game.Catalog.Constants;
using Turbo.Core.Game.Players;
using Turbo.Core.Utilities;
using Turbo.Packets.Outgoing.Catalog;

namespace Turbo.Catalog;

public class CatalogManager(
    ILogger<ICatalogManager> _logger,
    ICatalogFactory _catalogFactory) : Component, ICatalogManager
{
    public IDictionary<string, ICatalog> Catalogs { get; } = new Dictionary<string, ICatalog>();

    public ICatalogPage GetRootForPlayer(IPlayer player, string catalogType)
    {
        if (catalogType == null || catalogType == null) return null;

        if (Catalogs.TryGetValue(catalogType, out var catalog)) return catalog?.GetRootForPlayer(player) ?? null;

        return null;
    }

    public async Task<bool> PurchaseOfferForPlayer(IPlayer player, string catalogType, int pageId, int offerId,
        string extraParam, int quantity)
    {
        if (player == null || catalogType == null) return false;

        if (Catalogs.TryGetValue(catalogType, out var catalog))
        {
            var purchasedOffer = await catalog.PurchaseOffer(player, pageId, offerId, extraParam, quantity);

            if (purchasedOffer == null)
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
            return catalog.GetOfferForPlayer(player, offerId) ?? null;

        return null;
    }

    public ICatalogPage GetPageForPlayer(IPlayer player, string catalogType, int pageId)
    {
        if (player == null || catalogType == null) return null;

        if (Catalogs.TryGetValue(catalogType, out var catalog)) return catalog.GetPageForPlayer(player, pageId) ?? null;

        return null;
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
}