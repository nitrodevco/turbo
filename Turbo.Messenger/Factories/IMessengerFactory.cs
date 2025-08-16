using Turbo.Core.Game.Messenger;
using Turbo.Core.Game.Players;

namespace Turbo.Messenger.Factories;
public interface IMessengerFactory
{
    public IMessenger Create(IPlayer player);
}
