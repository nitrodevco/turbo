using Turbo.Core.EventHandlers;

namespace Turbo.Main.EventHandlers;

public class EventHandlerManager : IEventHandlerManager
{
    private readonly IEventHandler _EventHandler;

    public EventHandlerManager(
        IEventHandler EventHandler)
    {
        _EventHandler = EventHandler;
    }
}