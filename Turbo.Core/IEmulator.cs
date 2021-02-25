using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Core
{
    public interface IEmulator
    {
        public Task Start();
        public string GetVersion();
    }
}
