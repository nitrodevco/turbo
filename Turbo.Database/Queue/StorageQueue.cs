using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Turbo.Core.Storage;
using Turbo.Database.Context;
using Turbo.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Turbo.Database.Queue
{
    public class StorageQueue(IServiceScopeFactory _serviceScopeFactory) : IStorageQueue
    {
        private readonly IList<object> _entities = [];
        private readonly object _entityLock = new();

        public async ValueTask DisposeAsync()
        {
            await SaveNow();
        }

        public void Add(object entity)
        {
            lock (_entityLock)
            {
                if (_entities.Contains(entity)) return;

                _entities.Add(entity);
            }
        }

        public void AddAll(ICollection<object> entities)
        {
            lock (_entityLock)
            {
                foreach (var entity in entities)
                {
                    if (_entities.Contains(entity)) return;

                    _entities.Add(entity);
                }
            }
        }

        public async Task SaveNow()
        {
            List<object> entities = [];

            lock (_entityLock)
            {
                if (_entities.Count == 0) return;

                foreach (var entity in _entities) entities.Add(entity);

                _entities.Clear();
            }

            if (entities.Count == 0) return;

            using var scope = _serviceScopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<TurboContext>();

            foreach (var entity in entities) SaveEntity(entity, context);

            await context.SaveChangesAsync();

            context.ChangeTracker.Clear();
        }

        private void SaveEntity(object entity, TurboContext context)
        {
            if (entity == null || context == null) return;

            context.Attach(entity);

            var entry = context.Entry(entity);
            
            var isTemporary = entry.Property("Id").IsTemporary;

            entry.State = isTemporary ? EntityState.Added : EntityState.Modified;
        }
    }
}
