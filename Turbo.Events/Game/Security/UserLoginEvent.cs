using Turbo.Core.Game.Players;

namespace Turbo.Events.Game.Security;

public class UserLoginEvent : TurboEvent
{
    public IPlayer Player { get; init; }
}