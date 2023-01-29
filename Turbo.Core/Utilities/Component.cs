using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Turbo.Core.Utilities
{
    public abstract class Component : IComponent
    {
        public bool IsInitialized { get; private set; }
        public bool Disposed { get; private set; }
        public bool IsDisposing { get; private set; }

        public async ValueTask InitAsync()
        {
            if (IsInitialized) return;

            await OnInit();

            IsInitialized = true;
        }

        public async ValueTask DisposeAsync()
        {
            if (Disposed || IsDisposing) return;

            IsDisposing = true;

            await OnDispose();

            Disposed = true;
            IsDisposing = false;
        }

        protected abstract Task OnInit();

        protected abstract Task OnDispose();
    }
}