using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;

namespace Turbo.Core.Game.Catalog
{
    public interface ICatalog : IAsyncInitialisable, IAsyncDisposable
    {
        public string CatalogType { get; }
        public ICatalogPage GetRootForPlayer(IPlayer player);
        public ICatalogPage GetPageForPlayer(IPlayer player, int pageId, int offerId);
    }
}