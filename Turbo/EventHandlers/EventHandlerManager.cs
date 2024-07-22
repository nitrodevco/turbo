using Turbo.Core.EventHandlers;

namespace Turbo.EventHandlers;

public class EventHandlerManager : IEventHandlerManager
{
    private readonly IEventHandler _EventHandler;

    public EventHandlerManager(
        IEventHandler EventHandler)
    {
        _EventHandler = EventHandler;
    }
}