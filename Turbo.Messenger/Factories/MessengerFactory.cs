using Microsoft.Extensions.DependencyInjection;
using Turbo.Core.Game.Messenger;
using Turbo.Core.Game.Players;

namespace Turbo.Messenger.Factories;

public class MessengerFactory(IServiceProvider _provider) : IMessengerFactory
{
    public IMessenger Create(IPlayer player)
    {
        return ActivatorUtilities.CreateInstance<Messenger>(_provider, player);
    }
}