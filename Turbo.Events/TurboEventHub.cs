using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Turbo.Core.Events;

namespace Turbo.Events;

public class TurboEventHub : ITurboEventHub
{
    private readonly ILogger<ITurboEventHub> _logger;
    private readonly object _listenerLock;

    private readonly List<ITurboEventListener> _listeners;

    public TurboEventHub(ILogger<ITurboEventHub> logger)
    {
        _logger = logger;

        _listenerLock = new object();
        _listeners = new List<ITurboEventListener>();
    }

    public void Subscribe<T>(object subscriber, Action<T> handler) where T : ITurboEvent
    {
        SubscribeDelegate<T>(subscriber, handler);
    }

    public void Subscribe<T>(object subscriber, Func<T, Task> handler) where T : ITurboEvent
    {
        SubscribeDelegate<T>(subscriber, handler);
    }

    public T Dispatch<T>(T message) where T : ITurboEvent
    {
        var handlers = GetAliveHandlers<T>();

        foreach (var listener in handlers)
            switch (listener.Action)
            {
                case Action<T> action:
                    action(message);
                    break;
                case Func<T, Task> func:
                    func(message);
                    break;
            }

        return message;
    }

    public async Task<T> DispatchAsync<T>(T message) where T : ITurboEvent
    {
        foreach (var listener in GetAliveHandlers<T>())
            switch (listener.Action)
            {
                case Action<T> action:
                    action(message);
                    break;
                case Func<T, Task> func:
                    await func(message);
                    break;
            }

        return message;
    }

    private void SubscribeDelegate<T>(object subscriber, Delegate handler) where T : ITurboEvent
    {
        ITurboEventListener item = new TurboEventListener
        {
            Action = handler,
            Sender = new WeakReference(subscriber),
            Type = typeof(T)
        };

        lock (_listenerLock)
        {
            _listeners.Add(item);
        }
    }

    private List<ITurboEventListener> GetAliveHandlers<T>() where T : ITurboEvent
    {
        PruneHandlers();

        return _listeners.Where(h => h.Type.GetTypeInfo().IsAssignableFrom(typeof(T).GetTypeInfo())).ToList();
    }

    private void PruneHandlers()
    {
        lock (_listenerLock)
        {
            _listeners.RemoveAll(x => !x.Sender.IsAlive);
        }
    }
}