using System;
using System.Collections.Generic;
using System.Text;

namespace Turbo.Database.Context
{
    public interface IEmulatorContext : IDisposable
    {
        int SaveChanges();
    }
}
