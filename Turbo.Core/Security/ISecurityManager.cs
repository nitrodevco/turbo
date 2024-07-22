using System.Threading.Tasks;
using Turbo.Core.Utilities;

namespace Turbo.Core.Security;

public interface ISecurityManager : IComponent
{
    public Task<int> GetPlayerIdFromTicket(string ticket);
}