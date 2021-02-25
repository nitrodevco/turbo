using DotNetty.Transport.Bootstrapping;
using System.Threading.Tasks;

namespace Turbo.Networking
{
    public interface IServer
    {
        public string Host { get; }
        public int Port { get; }
        public void InitializeBoostrap();
        public Task StartAsync();
        public Task ShutdownAsync();
    }
}
