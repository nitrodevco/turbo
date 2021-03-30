using System.Collections.Generic;
using Turbo.Core.Game;

namespace Turbo.Core.Storage
{
    public interface IStorageQueue : ICyclable
    {
        public void Add(object entity);
        public void AddAll(ICollection<object> entities);
        public void SaveNow();
        public void Stop();
    }
}
