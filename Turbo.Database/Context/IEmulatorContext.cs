using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Turbo.Database.Entities;

namespace Turbo.Database.Context
{
    public interface IEmulatorContext : IDisposable
    {
        DbSet<Habbo> Habbos { get; set; }
        int SaveChanges();
    }
}
