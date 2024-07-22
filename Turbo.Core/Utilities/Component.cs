using System.Threading.Tasks;

namespace Turbo.Core.Utilities;

public abstract class Component : IComponent
{
    public bool IsInitialized { get; private set; }
    public bool IsInitializing { get; private set; }
    public bool IsDisposed { get; private set; }
    public bool IsDisposing { get; private set; }

    public async ValueTask InitAsync()
    {
        if (IsInitialized || IsInitializing) return;

        await OnInit();

        IsInitialized = true;
        IsInitializing = false;
    }

    public async ValueTask DisposeAsync()
    {
        if (IsDisposed || IsDisposing) return;

        IsDisposing = true;

        await OnDispose();

        IsDisposed = true;
        IsDisposing = false;
    }

    protected abstract Task OnInit();

    protected abstract Task OnDispose();
}