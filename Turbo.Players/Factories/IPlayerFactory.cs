using Turbo.Core.Game.Players;
using Turbo.Database.Entities.Players;

namespace Turbo.Players.Factories;

public interface IPlayerFactory
{
    public IPlayer Create(PlayerEntity playerEntity);
}