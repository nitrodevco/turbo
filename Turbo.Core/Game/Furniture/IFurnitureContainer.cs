using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Core.Game.Furniture
{
    public interface IFurnitureContainer
    {
        public IFurniture GetFurniture(int id);
        public void RemoveFurniture(int id);
        public void RemoveAllFurniture();
    }
}
