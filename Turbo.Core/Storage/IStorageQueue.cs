using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Turbo.Core.Storage;

public interface IStorageQueue : IAsyncDisposable
{
    public void Add(object entity);
    public void AddAll(ICollection<object> entities);
    public Task SaveEntityNow(object entity);
    public Task SaveNow();
}