using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game;

namespace Turbo.Core.Storage
{
    public interface IStorageQueue : IAsyncDisposable, ICyclable
    {
        public void Add(object entity);
        public void AddAll(ICollection<object> entities);
        public Task SaveNow();
    }
}
