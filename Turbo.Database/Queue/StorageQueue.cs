using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Storage;
using Turbo.Database.Context;

namespace Turbo.Database.Queue
{
    public class StorageQueue : IStorageQueue
    {
        private static readonly int _saveCycles = 20;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly ConcurrentQueue<object> _queue;

        private int _remainingSaveCycles = _saveCycles;

        public StorageQueue(IServiceScopeFactory scopeFactory)
        {
            _serviceScopeFactory = scopeFactory;

            _queue = new();
        }

        public async ValueTask DisposeAsync()
        {
            await SaveNow();
        }

        public void Add(object entity)
        {
            _queue.Enqueue(entity);
        }

        public void AddAll(ICollection<object> entities)
        {
            foreach (var entity in entities)
                _queue.Enqueue(entity);
        }

        public async Task SaveNow()
        {
            if (_queue.Count == 0) return;

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetService<IEmulatorContext>())
                {
                    while (_queue.TryDequeue(out object entity))
                    {
                        context.Update(entity);
                    }

                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task Cycle()
        {
            if (_remainingSaveCycles > -1)
            {
                if (_remainingSaveCycles == 0)
                {
                    await SaveNow();

                    _remainingSaveCycles = _saveCycles;

                    return;
                }

                _remainingSaveCycles--;
            }
        }
    }
}
