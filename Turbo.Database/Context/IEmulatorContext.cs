using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Turbo.Database.Entities;
using Turbo.Database.Entities.Players;

namespace Turbo.Database.Context
{
    public interface IEmulatorContext : IDisposable
    {
        DbSet<PlayerEntity> Players { get; set; }
        int SaveChanges();
    }
}
