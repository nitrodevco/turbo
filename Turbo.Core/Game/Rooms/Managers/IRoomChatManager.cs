using System.Threading.Tasks;
using Turbo.Core.Utilities;

namespace Turbo.Core.Game.Rooms.Managers
{
    public interface IRoomChatManager : IComponent
    {
        Task TryChat(uint userId, string text);
    }
}