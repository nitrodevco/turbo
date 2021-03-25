using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Furniture;
using Turbo.Database.Entities.Furniture;

namespace Turbo.Furniture.Factories
{
    public interface IFurnitureFactory
    {
        public IFurniture Create(IFurnitureContainer furnitureContainer, FurnitureEntity furnitureEntity);
    }
}
