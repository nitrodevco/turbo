namespace Turbo.Core.Events;

public interface ITurboEvent
{
    public bool IsCancelled { get; }

    public void Cancel();
}