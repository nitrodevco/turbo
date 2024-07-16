using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Turbo.Core.Configuration;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Constants;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Database.Repositories.Room;
using Turbo.Packets.Outgoing.Room.Chat;
using Turbo.Rooms.Object.Logic.Avatar;

namespace Turbo.Rooms.Managers
{
    public class ChatManager(
        IPlayerManager playerManager,
        ILogger<ChatManager> logger,
        IChatlogRepository chatlogRepository,
        IEmulatorConfig config)
        : IRoomChatManager
    {
        private readonly IPlayerManager _playerManager = playerManager ?? throw new ArgumentNullException(nameof(playerManager));
        private readonly ILogger<ChatManager> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IChatlogRepository _chatlogRepository = chatlogRepository ?? throw new ArgumentNullException(nameof(chatlogRepository));
        private readonly IEmulatorConfig _config = config ?? throw new ArgumentNullException(nameof(config));

        private IRoom _room;

        private ConcurrentDictionary<int, FloodControlData> _playerFloodData = new();

        public void SetRoom(IRoom room)
        {
            _room = room ?? throw new ArgumentNullException(nameof(room));
        }

        public async Task TryChat(uint userId, string text, RoomChatType chatType, int? recipientId = null)
        {
            await ProcessChat(userId, text, chatType, recipientId);
        }

        private async Task ProcessChat(uint userId, string text, RoomChatType chatType, int? recipientId = null)
        {
            var player = _playerManager.GetPlayerById((int)userId);
    
            if (_room == null || userId <= 0 || player == null)
            {
                _logger.LogWarning("Issue processing chat message. Room or player not found.");
                return;
            }

            if (_room.RoomSecurityManager.TryGetPlayerMuteRemainingTime(player, out var remainingMuteTime))
            {
                await player.Session.Send(new RemaningMutePeriodMessage { MuteSecondsRemaining = (int)remainingMuteTime.TotalSeconds });
                return;
            }

            if (IsPlayerFlooding(player))
            {
                await player.Session.Send(new FloodControlMessage { Seconds = _config.FloodMuteDurationSeconds });
                return;
            }

            var roomObject = player.RoomObject;
            if (roomObject?.Room != _room || roomObject.Logic is not PlayerLogic playerLogic)
            {
                _logger.LogWarning($"Player with userId: {userId} is not in the room or PlayerLogic not found.");
                return;
            }

            switch (chatType)
            {
                case RoomChatType.Normal:
                    playerLogic.Say(text);
                    break;
                case RoomChatType.Whisper:
                    if (recipientId.HasValue)
                    {
                        var recipientPlayer = _playerManager.GetPlayerById(recipientId.Value);
                        if (recipientPlayer?.RoomObject != null)
                        {
                            playerLogic.Whisper(text, recipientPlayer);
                        }
                    }
                    break;
                case RoomChatType.Shout:
                    playerLogic.Shout(text);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(chatType), chatType, null);
            }

            await _chatlogRepository.AddChatlogAsync(
                _room.Id,
                player.Id,
                text,
                chatType.ToString().ToLower(),
                DateTime.Now,
                DateTime.Now,
                recipientId
            );
        }
        
        private bool IsPlayerFlooding(IPlayer player)
        {
            var now = DateTime.UtcNow;

            if (!_playerFloodData.TryGetValue(player.Id, out var floodData))
            {
                floodData = new FloodControlData { MessageCount = 1, FirstMessageTime = now };
                _playerFloodData[player.Id] = floodData;
                return false;
            }

            var timeSinceFirstMessage = (now - floodData.FirstMessageTime).TotalSeconds;

            if (timeSinceFirstMessage > _config.FloodTimeFrameSeconds)
            {
                floodData.MessageCount = 1;
                floodData.FirstMessageTime = now;
            }
            else
            {
                floodData.MessageCount++;

                if (floodData.MessageCount > _config.FloodMessageLimit)
                {
                    _playerFloodData[player.Id] = new FloodControlData();
                    return true;
                }
            }

            _playerFloodData[player.Id] = floodData;
            return false;
        }

        public async Task SetChatStylePreference(uint userId, int styleId)
        {
            var player = _playerManager.GetPlayerById((int)userId);
            if (player == null)
            {
                _logger.LogWarning($"Player with userId: {userId} not found.");
                return;
            }

            var playerSettings = player.PlayerSettings;

            if (styleId == 0 || playerSettings.OwnedChatStyles.Contains(styleId))
            {
                if (playerSettings.ChatStyle == styleId) return;

                playerSettings.ChatStyle = styleId;
                await _playerManager.SaveSettings(playerSettings);
            }
            else
            {
                await player.Session.Send(new WhisperMessage
                {
                    ObjectId = player.Id,
                    Text = "You don't own this chat style. Keeping your current chat style.",
                    Gesture = 0,
                    StyleId = playerSettings.ChatStyle,
                    Links = new List<string>(),
                    AnimationLength = 0
                });
            }
        }
    }
}