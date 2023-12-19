using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Catalog;
using Turbo.Core.Game.Players;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Database.Entities.Catalog;
using Turbo.Packets.Outgoing.Catalog;

namespace Turbo.Catalog
{
    public class CatalogPage : ICatalogPage
    {
        private readonly ILogger<ICatalogPage> _logger;
        private readonly CatalogPageEntity _entity;

        public ICatalogPage Parent { get; private set; }
        public IDictionary<int, ICatalogPage> Children { get; private set; }
        public IDictionary<int, ICatalogOffer> Offers { get; private set; }
        public IList<int> OfferIds { get; private set; }
        public IList<string> ImageDatas { get; private set; }
        public IList<string> TextDatas { get; private set; }

        public CatalogPage(
            ILogger<ICatalogPage> logger,
            CatalogPageEntity entity)
        {
            _logger = logger;
            _entity = entity;

            Children = new Dictionary<int, ICatalogPage>();
            Offers = new Dictionary<int, ICatalogOffer>();
            OfferIds = new List<int>();
            ImageDatas = new List<string>();
            TextDatas = new List<string>();
        }

        public virtual void SetParent(ICatalogPage catalogPage)
        {
            if ((catalogPage == null) || (catalogPage == Parent)) return;

            Parent = catalogPage;

            Parent.AddChild(this);
        }

        public virtual void AddChild(ICatalogPage catalogPage)
        {
            if ((catalogPage == null) || Children.ContainsKey(catalogPage.Id)) return;

            Children.Add(catalogPage.Id, catalogPage);

            catalogPage.SetParent(this);
        }

        public virtual void AddOffer(ICatalogOffer catalogOffer)
        {
            if ((catalogOffer == null) || Offers.ContainsKey(catalogOffer.Id)) return;

            Offers.Add(catalogOffer.Id, catalogOffer);

            catalogOffer.SetPage(this);
        }

        public virtual void CacheOfferIds()
        {
            OfferIds = Offers.Keys.ToList();
        }

        public async Task<ICatalogOffer> PurchaseOffer(IPlayer player, int offerId, string extraParam, int quantity)
        {
            if(player == null) return null;

            if(Offers.TryGetValue(offerId, out var offer))
            {
                return await offer.Purchase(player, extraParam, quantity);
            }

            return null;
        }

        public virtual int Id => _entity.Id;

        public virtual int ParentId => _entity.ParentEntityId ?? -1;
        public virtual int Icon => _entity.Icon;
        public virtual string Name => _entity.Name;
        public virtual string Localization => _entity.Localization;
        public virtual string Layout => _entity.Layout;
        public virtual bool Visible => _entity.Visible ?? false;
    }
}