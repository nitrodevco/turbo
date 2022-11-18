using System;
using Turbo.Core.Game.Furniture;
using Turbo.Database.Entities.Furniture;

namespace Turbo.Furniture.Factories
{
	public interface IPlayerFurnitureFactory
	{
        public IPlayerFurniture Create(FurnitureEntity furnitureEntity);

    }
}

