using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Core.Utilities;

namespace Turbo.Core.Game.Catalog
{
    public interface ICatalogManager : IComponent
    {
        public IDictionary<string, ICatalog> Catalogs { get; }
        public ICatalogPage GetRootForPlayer(IPlayer player, string catalogType);
        public Task PurchaseOffer(IPlayer player, int pageId, int offerId, string extraParam, int quantity);
    }
}
