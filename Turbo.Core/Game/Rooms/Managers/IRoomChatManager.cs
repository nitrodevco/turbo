using System.Threading.Tasks;

namespace Turbo.Core.Game.Rooms.Managers;

public interface IRoomChatManager
{
    Task TryRoomChat(uint userId, string text, string type, bool isShout = false);
    Task TryWhisperChat(uint userId, int recipientId, string text, string type);
    Task SetChatStylePreference(uint userId, int styleId);
}