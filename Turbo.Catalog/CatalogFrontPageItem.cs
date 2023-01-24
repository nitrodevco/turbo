using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Game.Catalog;
using Turbo.Core.Game.Catalog.Constants;

namespace Turbo.Catalog
{
    public class CatalogFrontPageItem : ICatalogFrontPageItem
    {
        public int Position { get; private set; }
        public string Name { get; private set; }
        public string PromoImage { get; private set; }
        public FrontPageItemType Type { get; private set; }
        public DateTime Expiration { get; private set; }
    }
}