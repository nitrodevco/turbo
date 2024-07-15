using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Database.Repositories.Room;
using Turbo.Rooms.Object.Logic.Avatar;

namespace Turbo.Rooms.Managers
{
    public class ChatManager(
        IPlayerManager playerManager,
        ILogger<ChatManager> logger,
        IChatlogRepository chatlogRepository)
        : IRoomChatManager
    {
        private readonly IPlayerManager _playerManager = playerManager ?? throw new ArgumentNullException(nameof(playerManager));
        private readonly ILogger<ChatManager> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IChatlogRepository _chatlogRepository = chatlogRepository ?? throw new ArgumentNullException(nameof(chatlogRepository));

        private IRoom _room;

        public void SetRoom(IRoom room)
        {
            _room = room ?? throw new ArgumentNullException(nameof(room));
        }

        public async Task TryRoomChat(uint userId, string text, string type)
        {
            await ProcessChat(userId, text, type, false, null);
        }

        public async Task TryWhisperChat(uint userId, int recipientId, string text, string type)
        {
            await ProcessChat(userId, text, type, true, recipientId);
        }

        private async Task ProcessChat(uint userId, string text, string type, bool isWhisper, int? recipientId)
        {
            if (_room == null)
            {
                _logger.LogWarning("Room is not set in ChatManager.");
                return;
            }

            if (userId <= 0)
            {
                _logger.LogWarning("Invalid userId provided to TryChat.");
                return;
            }

            var player = _playerManager.GetPlayerById((int)userId);

            if (player == null)
            {
                _logger.LogWarning($"Player with userId: {userId} not found.");
                return;
            }

            var roomObject = player.RoomObject;

            if (roomObject == null || roomObject.Room != _room)
            {
                _logger.LogWarning($"Player with userId: {userId} is not in the room.");
                return;
            }

            if (roomObject.Logic is PlayerLogic playerLogic)
            {
                _logger.LogInformation($"Player with userId: {userId} sending chat message.");
                
                if (isWhisper && recipientId.HasValue)
                {
                    var recipientPlayer = _playerManager.GetPlayerById(recipientId.Value);
                    if (recipientPlayer?.RoomObject != null)
                    {
                        playerLogic.Whisper(text, recipientPlayer);
                    }
                }
                else
                {
                    playerLogic.Say(text);
                }

                await _chatlogRepository.AddChatlogAsync(
                    _room.Id,
                    player.Id,
                    text,
                    type,
                    DateTime.Now,
                    DateTime.Now,
                    recipientId
                );
            }
            else
            {
                _logger.LogWarning($"PlayerLogic not found for player with userId: {userId}.");
            }
        }
        
        public async Task SetChatStylePreference(uint userId, int styleId)
        {
            var player = _playerManager.GetPlayerById((int)userId);

            if (player != null)
            {
                player.PlayerSettings.ChatStyle = styleId;
                await _playerManager.SaveSettings(player.PlayerSettings);
                _logger.LogInformation($"Player with userId: {userId} changed chat style to: {styleId}.");
            }
            else
            {
                _logger.LogWarning($"Player with userId: {userId} not found.");
            }
        }
    }
}