using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Storage;
using Turbo.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace Turbo.Database.Queue;

public class StorageQueue(IServiceScopeFactory serviceScopeFactory) : IStorageQueue
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
    private readonly ConcurrentQueue<object> _entities = new();

    public async ValueTask DisposeAsync()
    {
        await SaveNow();
        GC.SuppressFinalize(this);
    }

    public void Add(object entity)
    {
        if (!_entities.Contains(entity))
        {
            _entities.Enqueue(entity);
        }
    }

    public void AddAll(ICollection<object> entities)
    {
        foreach (var entity in entities)
        {
            if (!_entities.Contains(entity))
            {
                _entities.Enqueue(entity);
            }
        }
    }

    public async Task SaveNow()
    {
        if (_entities.IsEmpty) return;

        using var scope = _serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TurboContext>();

        while (_entities.TryDequeue(out var entity))
        {
            SaveEntity(entity, context);
        }

        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
    }

    private void SaveEntity(object entity, TurboContext context)
    {
        if (entity == null || context == null) return;

        context.Attach(entity);
        var entry = context.Entry(entity);
        entry.State = EntityState.Modified;
    }
}