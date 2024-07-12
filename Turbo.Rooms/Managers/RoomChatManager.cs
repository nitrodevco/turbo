using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Storage;
using Turbo.Core.Utilities;
using Turbo.Database.Entities.Room;
using Turbo.Rooms.Object.Logic.Avatar;

namespace Turbo.Rooms.Managers
{
    public class RoomChatManager(IRoom room, IPlayerManager playerManager, ILogger<RoomChatManager> logger, IStorageQueue storageQueue)
        : Component, IRoomChatManager
    {
    private readonly IRoom _room = room ?? throw new ArgumentNullException(nameof(room));
    private readonly IPlayerManager _playerManager = playerManager ?? throw new ArgumentNullException(nameof(playerManager));
    private readonly ILogger<RoomChatManager> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IStorageQueue _storageQueue = storageQueue ?? throw new ArgumentNullException(nameof(storageQueue));
    
    public Task TryChat(uint userId, string text)
    {
        if (userId <= 0)
        {
            _logger.LogWarning("Invalid userId provided to TryChat.");
            return Task.CompletedTask;
        }

        var player = _playerManager.GetPlayerById((int)userId);

        if (player == null)
        {
            _logger.LogWarning($"Player with userId: {userId} not found.");
            return Task.CompletedTask;
        }

        var roomObject = player.RoomObject;

        if (roomObject == null || roomObject.Room != _room)
        {
            _logger.LogWarning($"Player with userId: {userId} is not in the room.");
            return Task.CompletedTask;
        }

        if (roomObject.Logic is PlayerLogic playerLogic)
        {
            _logger.LogInformation($"Player with userId: {userId} sending chat message.");
            playerLogic.Say(text);
            
            var chatLog = new RoomChatLogEntity
            {
                RoomEntityId = _room.Id,
                PlayerEntityId = player.Id,
                Message = text,
                DateCreated = DateTime.UtcNow
            };
            _storageQueue.Add(chatLog);
        }
        else
        {
            _logger.LogWarning($"PlayerLogic not found for player with userId: {userId}.");
        }

        return Task.CompletedTask;
    }
    
    protected override Task OnInit()
    {
        _logger.LogInformation("RoomChatManager initialized.");
        return Task.CompletedTask;
    }

    protected override Task OnDispose()
    {
        _logger.LogInformation("RoomChatManager disposed.");
        return Task.CompletedTask;
    }
}

}