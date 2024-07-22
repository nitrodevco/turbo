using System;

namespace Turbo.Packets;

public interface IListener
{
    public Delegate Action { get; set; }
    public WeakReference Sender { get; set; }
    public Type Type { get; set; }
}