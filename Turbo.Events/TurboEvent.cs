using Turbo.Core.Events;

namespace Turbo.Events;

public class TurboEvent : ITurboEvent
{
    public bool IsCancelled { get; private set; }

    public void Cancel()
    {
        IsCancelled = true;
    }
}