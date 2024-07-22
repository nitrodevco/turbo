using System;

namespace Turbo.Core.Utilities;

public interface IComponent : IAsyncInitialisable, IAsyncDisposable
{
    public bool IsInitialized { get; }
    public bool IsInitializing { get; }
    public bool IsDisposed { get; }
    public bool IsDisposing { get; }
}