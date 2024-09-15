using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Turbo.Core.Configuration;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Constants;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Storage;
using Turbo.Core.Utilities;
using Turbo.Core.Database.Entities.Room;
using Turbo.Database.Repositories.Room;
using Turbo.Packets.Outgoing.Room.Chat;

namespace Turbo.Rooms.Managers;

public class RoomChatManager(
    ILogger<RoomChatManager> _logger,
    IPlayerManager _playerManager,
    IEmulatorConfig _config,
    IRoom _room,
    IServiceScopeFactory _serviceScopeFactory,
    IStorageQueue _storageQueue)
    : Component, IRoomChatManager
{
    private static readonly int DefaultChatStyleClientId = 1;
    private readonly ConcurrentDictionary<int, FloodControlData> _playerFloodData = new();

    public IDictionary<int, DateTime> Mutes { get; } = new ConcurrentDictionary<int, DateTime>();

    public bool IsPlayerMuted(IPlayer player)
    {
        if (player == null || !Mutes.ContainsKey(player.Id)) return false;

        var isOwner = _room.RoomSecurityManager.IsOwner(player);

        if (!Mutes.TryGetValue(player.Id, out var expiration)) return false;

        if (!isOwner && DateTime.Compare(DateTime.Now, expiration) < 0) return true;

        Mutes.Remove(player.Id);

        return false;
    }

    public TimeSpan GetPlayerRemainingMuteTime(IPlayer player)
    {
        var remainingTime = TimeSpan.Zero;

        if (player != null)
            if (Mutes.TryGetValue(player.Id, out var expiration))
                if (DateTime.Compare(DateTime.Now, expiration) < 0)
                    remainingTime = expiration - DateTime.Now;

        return remainingTime;
    }

    public void SendChatForPlayer(IPlayer player, string message, int chatStyleId = -1)
    {
        SendMessageForPlayer(player, message, RoomChatType.Normal, chatStyleId);
    }

    public void SendWhisperForPlayer(IPlayer player, string targetPlayerName, string message, int chatStyleId = -1)
    {
        if (string.IsNullOrEmpty(targetPlayerName)) return;

        var targetRoomObject = _room.RoomUserManager.GetRoomObjectByUsername(targetPlayerName);

        if (targetRoomObject == null || targetRoomObject.RoomObjectHolder is not IPlayer targetPlayer) return;

        SendMessageForPlayer(player, message, RoomChatType.Whisper, chatStyleId, targetPlayer);
    }

    public void SendShoutForPlayer(IPlayer player, string message, int chatStyleId = -1)
    {
        SendMessageForPlayer(player, message, RoomChatType.Shout, chatStyleId);
    }

    public void SendMessageFromRoomObject(int roomObjectId, string message, RoomChatType chatType = RoomChatType.Normal,
        int chatStyleId = -1, int? targetRoomObjectId = null)
    {
        var roomObject = _room.RoomUserManager.AvatarObjects.GetRoomObject(roomObjectId);
        var targetRoomObject = targetRoomObjectId.HasValue
            ? _room.RoomUserManager.AvatarObjects.GetRoomObject(targetRoomObjectId.Value)
            : null;

        SendMessageFromRoomObject(roomObject, message, chatType, chatStyleId, targetRoomObject);
    }

    public void SendMessageFromRoomObject(IRoomObjectAvatar roomObject, string message,
        RoomChatType chatType = RoomChatType.Normal, int chatStyleId = -1, IRoomObjectAvatar targetRoomObject = null)
    {
        if (chatStyleId == -1) chatStyleId = DefaultChatStyleClientId;

        // TODO still need to implement hearing distance

        switch (chatType)
        {
            case RoomChatType.Normal:
                _room.SendComposer(new ChatMessage
                {
                    ObjectId = roomObject.Id,
                    Text = message,
                    Gesture = 0,
                    StyleId = chatStyleId,
                    Links = [],
                    AnimationLength = 0
                });

                break;
            case RoomChatType.Whisper:
                var whisperMessage = new WhisperMessage
                {
                    ObjectId = roomObject.Id,
                    Text = message,
                    Gesture = 0,
                    StyleId = chatStyleId,
                    Links = [],
                    AnimationLength = 0
                };

                if (targetRoomObject != null && targetRoomObject.RoomObjectHolder is IPlayer targetPlayer)
                    targetPlayer.Session?.Send(whisperMessage);

                if (roomObject.RoomObjectHolder is IPlayer roomObjectPlayer)
                    roomObjectPlayer.Session?.Send(whisperMessage);

                break;
            case RoomChatType.Shout:
                _room.SendComposer(new ShoutMessage
                {
                    ObjectId = roomObject.Id,
                    Text = message,
                    Gesture = 0,
                    StyleId = chatStyleId,
                    Links = [],
                    AnimationLength = 0
                });
                break;
        }
    }

    protected override async Task OnInit()
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var roomMuteRepository = scope.ServiceProvider.GetService<IRoomMuteRepository>();
        var muteEntities = await roomMuteRepository.FindAllByRoomIdAsync(_room.Id);

        foreach (var entity in muteEntities)
        {
            if (DateTime.Compare(DateTime.Now, entity.DateExpires) >= 0)
            {
                await roomMuteRepository.RemoveMuteEntityAsync(entity);

                continue;
            }

            Mutes.Add(entity.PlayerEntityId, entity.DateExpires);
        }
    }

    protected override async Task OnDispose()
    {
        Mutes.Clear();
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

    private string ProcessMessageSanitation(string message)
    {
        return message;
    }

    private void SendMessageForPlayer(IPlayer player, string message, RoomChatType chatType, int chatStyleId = -1,
        IPlayer targetPlayer = null)
    {
        if (player == null || string.IsNullOrEmpty(message)) return;

        var roomObject = player.RoomObject;

        if (roomObject == null || roomObject.Room != _room) return;

        var remainingMuteTime = GetPlayerRemainingMuteTime(player);

        if (remainingMuteTime > TimeSpan.Zero)
        {
            player.Session?.Send(new RemaningMutePeriodMessage
            {
                MuteSecondsRemaining = (int)remainingMuteTime.TotalSeconds
            });

            return;
        }

        if (IsPlayerFlooding(player))
        {
            player.Session?.Send(new FloodControlMessage
            {
                Seconds = _config.FloodMuteDurationSeconds
            });

            return;
        }

        message = ProcessMessageSanitation(message);

        if (string.IsNullOrEmpty(message)) return;

        chatStyleId = player.PlayerDetails.GetValidChatStyleId(chatStyleId);

        if (chatType == RoomChatType.Whisper)
            if (targetPlayer == null || targetPlayer.RoomObject == null || targetPlayer.RoomObject.Room != _room)
                return;

        _storageQueue.Add(new RoomChatlogEntity
        {
            RoomEntityId = _room.Id,
            PlayerEntityId = player.Id,
            Message = message,
            TargetPlayerEntityId = targetPlayer?.Id ?? null
        });

        SendMessageFromRoomObject(roomObject, message, chatType, chatStyleId, targetPlayer?.RoomObject);
    }
}