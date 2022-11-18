using System;
namespace Turbo.Core.Game.Furniture
{
	public interface IPlayerFurniture
	{
		public int Id { get; }
		public int PlayerId { get; }
	}
}

