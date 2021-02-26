using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Core
{
    public interface IEmulator : IHostedService
    {
        public string GetVersion();
    }
}
