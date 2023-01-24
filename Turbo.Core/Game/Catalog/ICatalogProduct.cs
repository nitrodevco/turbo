using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Game.Furniture.Definition;

namespace Turbo.Core.Game.Catalog
{
    public interface ICatalogProduct
    {
        public void SetOffer(ICatalogOffer catalogOffer);
        public void SetFurnitureDefinition(IFurnitureDefinition furnitureDefinition);

        public int Id { get; }
        public int OfferId { get; }
        public int FurnitureDefinitionId { get; }
        public string ProductType { get; }
        public int SpriteId { get; }
        public string ExtraParam { get; }
        public int Quantity { get; }
        public int UniqueSize { get; }
        public int UniqueRemaining { get; }
    }
}
