using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Constants;

namespace Turbo.Core.Game.Rooms.Managers;

public interface IRoomChatManager
{
    Task TryChat(uint userId, string text, RoomChatType chatType, int? recipientId = null);
    Task SetChatStylePreference(uint userId, int styleId);
}