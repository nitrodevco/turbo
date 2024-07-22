using System;
using System.Collections.Generic;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms.Constants;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Utilities;

namespace Turbo.Core.Game.Rooms.Managers;

public interface IRoomChatManager : IComponent
{
    public IDictionary<int, DateTime> Mutes { get; }
    public bool IsPlayerMuted(IPlayer player);
    public TimeSpan GetPlayerRemainingMuteTime(IPlayer player);
    public void SendChatForPlayer(IPlayer player, string message, int chatStyleId = -1);
    public void SendWhisperForPlayer(IPlayer player, string targetPlayerName, string message, int chatStyleId = -1);
    public void SendShoutForPlayer(IPlayer player, string message, int chatStyleId = -1);

    public void SendMessageFromRoomObject(int roomObjectId, string message, RoomChatType chatType = RoomChatType.Normal,
        int chatStyleId = -1, int? targetRoomObjectId = null);

    public void SendMessageFromRoomObject(IRoomObjectAvatar roomObject, string message,
        RoomChatType chatType = RoomChatType.Normal, int chatStyleId = -1, IRoomObjectAvatar targetRoomObject = null);
}