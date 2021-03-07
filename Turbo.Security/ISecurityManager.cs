using System;
using System.Threading.Tasks;
using Turbo.Core;

namespace Turbo.Security
{
    public interface ISecurityManager : IAsyncInitialisable, IAsyncDisposable
    {
        public Task<int> GetPlayerIdFromTicket(string ticket);
    }
}
