using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Database.Repositories
{
    public interface IRepository<T>
    {
        T Find(int id);
    }
}
