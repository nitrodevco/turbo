using Turbo.Core.EventHandlers;
using Turbo.Core.Events;
using Turbo.Events.Game.Security;
using Turbo.Packets.Outgoing.Navigator;

namespace Turbo.EventHandlers;

public class UserLoginEventHandler : IEventHandler
{
    private readonly ITurboEventHub _eventHub;

    public UserLoginEventHandler(ITurboEventHub eventHub)
    {
        _eventHub = eventHub;

        _eventHub.Subscribe<UserLoginEvent>(this, UserLoginEvent);
    }

    public void UserLoginEvent(UserLoginEvent user)
    {
        user.Player.Session.Send(new NavigatorSettingsMessage
        {
            RoomIdToEnter = 1,
            HomeRoomId = 1
        });
    }
}