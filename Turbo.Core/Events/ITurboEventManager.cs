using System;
using System.Threading.Tasks;

namespace Turbo.Core.Events;

public interface ITurboEventHub
{
    public void Subscribe<T>(object subscriber, Action<T> handler) where T : ITurboEvent;
    public void Subscribe<T>(object subscriber, Func<T, Task> handler) where T : ITurboEvent;
    public T Dispatch<T>(T message) where T : ITurboEvent;
    public Task<T> DispatchAsync<T>(T message) where T : ITurboEvent;
}