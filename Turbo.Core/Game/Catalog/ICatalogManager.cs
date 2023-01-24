using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;

namespace Turbo.Core.Game.Catalog
{
    public interface ICatalogManager : IAsyncInitialisable, IAsyncDisposable
    {
        public IDictionary<string, ICatalog> Catalogs { get; }
        public ICatalogPage GetRootForPlayer(IPlayer player, string catalogType);
        public ValueTask PurchaseOffer(IPlayer player, int pageId, int offerId, string extraParam, int quantity);
    }
}
