using Turbo.Core.Game.Messenger;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Players.Constants;
using Turbo.Core.Game.Rooms.Object.Constants;

namespace Turbo.Messenger;
public class MessengerSearchResult(IPlayer player) : IMessengerSearchResult
{
    public int Id => player.Id;
    public string Name => player.Name;
    public string Motto => player.Motto;
    public string Figure => player.Figure;
    public PlayerStatusEnum Status => player.PlayerDetails.PlayerStatus;
    public AvatarGender Gender => player.Gender;
    public DateTime LastOnline => player.PlayerDetails.UpdatedAt;
    public bool CanFollow => player.RoomObject is not null;
    public string RealName => "";
}
