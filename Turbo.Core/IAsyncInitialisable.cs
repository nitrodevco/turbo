using System.Threading.Tasks;

namespace Turbo.Core;

public interface IAsyncInitialisable
{
    public ValueTask InitAsync();
}