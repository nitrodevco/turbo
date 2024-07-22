using System;
using Turbo.Core.Events;

namespace Turbo.Events;

public class TurboEventListener : ITurboEventListener
{
    public Delegate Action { get; set; }
    public WeakReference Sender { get; set; }
    public Type Type { get; set; }
}