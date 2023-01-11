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
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly ConcurrentQueue<object> _queue;

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

        public async Task SaveNow(object entity)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetService<IEmulatorContext>())
                {
                    context.Update(entity);

                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task Cycle()
        {
            await SaveNow();
        }
    }
}
