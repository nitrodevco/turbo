using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Networking;

namespace Turbo.Networking;

public interface IServerManager
{
    public List<IServer> Servers { get; }
    public Task StartServersAsync();
    public Task ShutdownServersAsync();
}