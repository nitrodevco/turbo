using System;
using Microsoft.Extensions.DependencyInjection;
using Turbo.Core.Game.Players;
using Turbo.Core.Database.Entities.Players;
using Turbo.Core.Database.Factories;
using Turbo.Core.Database.Factories.Players;
using Turbo.Inventory.Factories;

namespace Turbo.Players.Factories;

public class PlayerFactory(
    IPlayerInventoryFactory _playerInventoryFactory,
    IServiceScopeFactory _serviceScopeFactory,
    IServiceProvider _provider) : IPlayerFactory
{
    public IPlayer Create(PlayerEntity playerEntity)
    {
        var playerDetails = ActivatorUtilities.CreateInstance<PlayerDetails>(_provider, playerEntity);
        var player = ActivatorUtilities.CreateInstance<Player>(_provider, playerDetails);
        var inventory = _playerInventoryFactory.Create(player);
        var wallet = new PlayerWallet(player, _serviceScopeFactory);
        var perks = new PlayerPerks(player, _serviceScopeFactory);
        

        player.SetInventory(inventory);
        player.SetWallet(wallet);
        player.SetPerks(perks);

        return player;
    }
}