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
    public class StorageQueue : IStorageQueue
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IList<object> _entities;
        private readonly object _entityLock;

        public StorageQueue(IServiceScopeFactory scopeFactory)
        {
            _serviceScopeFactory = scopeFactory;
            _entities = new List<object>();
            _entityLock = new object();
        }

        public async ValueTask DisposeAsync()
        {
            await SaveNow();
        }

        public void Add(object entity)
        {
            lock(_entityLock)
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
            List<object> entities = new();

            lock(_entityLock)
            {
                if (_entities.Count == 0) return;

                foreach (var entity in _entities) entities.Add(entity);

                _entities.Clear();
            }

            if (entities.Count == 0) return;

            using var scope = _serviceScopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetService<TurboDbContext>();

            foreach (var entity in entities) SaveEntity(entity, context);

            await context.SaveChangesAsync();

            context.ChangeTracker.Clear();
        }

        public async Task SaveNow(object entity)
        {
            lock (_entityLock)
            {
                if(_entities.Contains(entity)) _entities.Remove(entity);
            }

            using var scope = _serviceScopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetService<TurboDbContext>();

            SaveEntity(entity, context);

            await context.SaveChangesAsync();

            context.Entry(entity).State = EntityState.Detached;
        }

        private void SaveEntity(object entity, TurboDbContext context)
        {
            if(entity == null || context == null) return;

            context.Attach(entity);

            var entry = context.Entry(entity);

            foreach(var property in entry.OriginalValues.Properties)
            {
                var originalValue = entry.OriginalValues[property];
                var currentValue = entry.CurrentValues[property];

                System.Console.WriteLine(property.Name + " " + originalValue + " " + currentValue);

                if(!object.Equals(originalValue, currentValue)) entry.Property(property.Name).IsModified = true;
            }
        }

        public async Task Cycle()
        {
            await SaveNow();
        }
    }
}
