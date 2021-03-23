using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Turbo.Core.Game;
using Turbo.Database.Context;
using Turbo.Database.Entities;

namespace Turbo.Database.Queue
{
    public class StorageQueue<T> : ICyclable 
        where T : Entity
    {
        private readonly ConcurrentQueue<T> _queue;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly int _cycleIntervalMs;
        private readonly CancellationTokenSource _cancellationToken;
        private bool _running;
        private Task _cycle;

        public StorageQueue(int intervalMs, IServiceScopeFactory scopeFactory)
        {
            _serviceScopeFactory = scopeFactory;
            _queue = new ConcurrentQueue<T>();
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

        public void Add(T entity)
        {
            _queue.Enqueue(entity);
        }

        public void AddAll(Collection<T> entities)
        {
            foreach (var entity in entities)
                _queue.Enqueue(entity);
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
                    while (_queue.TryDequeue(out T entity))
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
