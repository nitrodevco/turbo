using Turbo.Core.Database.Entities.Players;
using Turbo.Core.Game.Players;

namespace Turbo.Core.Database.Factories.Players;

public interface IPlayerFactory
{
    public IPlayer Create(PlayerEntity playerEntity);
}