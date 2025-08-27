using System;
using Microsoft.Extensions.DependencyInjection;
using Turbo.Core.Game.Players;
using Turbo.Database.Entities.Players;
using Turbo.Inventory.Factories;
using Turbo.Messenger.Factories;

namespace Turbo.Players.Factories;

public class PlayerFactory(
    IPlayerInventoryFactory _playerInventoryFactory,
    IMessengerFactory _messengerFactory,
    IServiceScopeFactory _serviceScopeFactory,
    IServiceProvider _provider) : IPlayerFactory
{
    public IPlayer Create(PlayerEntity playerEntity)
    {
        var playerDetails = ActivatorUtilities.CreateInstance<PlayerDetails>(_provider, playerEntity);
        var player = ActivatorUtilities.CreateInstance<Player>(_provider, playerDetails);
        var inventory = _playerInventoryFactory.Create(player);
        var wallet = new PlayerWallet(player, _serviceScopeFactory);
        var messenger = _messengerFactory.Create(player);

        player.SetInventory(inventory);
        player.SetWallet(wallet);
        player.SetMessenger(messenger);

        return player;
    }
}