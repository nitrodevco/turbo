using System;
using System.Threading.Tasks;

namespace Turbo.Core.Security
{
    public interface ISecurityManager : IAsyncInitialisable, IAsyncDisposable
    {
        public Task<int> GetPlayerIdFromTicket(string ticket);
    }
}
