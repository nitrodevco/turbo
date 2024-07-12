using System.Threading.Tasks;

namespace Turbo.Core.Game.Rooms.Managers;

public interface IRoomChatManager
{
    Task TryRoomChat(uint userId, string text, string type);
}