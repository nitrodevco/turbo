using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Turbo.Core.Storage;
using Turbo.Database.Context;

namespace Turbo.Database.Queue
{
    public class StorageQueue : IStorageQueue
    {
        private readonly ConcurrentQueue<object> _queue;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly int _cycleIntervalMs;
        private readonly CancellationTokenSource _cancellationToken;
        private bool _running;
        private Task _cycle;

        public StorageQueue(int intervalMs, IServiceScopeFactory scopeFactory)
        {
            _serviceScopeFactory = scopeFactory;
            _queue = new ConcurrentQueue<object>();
            _cancellationToken = new();
            _cycleIntervalMs = intervalMs;

            _cycle = Task.Run(async () =>
            {
                while (!_cancellationToken.IsCancellationRequested)
                {
                    _running = true;
                    await Cycle();
                    _running = false;
                    await Task.Delay(_cycleIntervalMs);
                }
            });
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

        public void SaveNow()
        {
            if (!_running) Cycle();
        }

        public void Stop()
        {
            if (!_running) Cycle();
            _cancellationToken.Cancel();
        }

        public Task Cycle()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetService<IEmulatorContext>())
                {
                    while (_queue.TryDequeue(out object entity))
                    {
                        context.Update(entity);
                    }
                    context.SaveChanges();
                }
            }
            return Task.CompletedTask;
        }
    }
}
