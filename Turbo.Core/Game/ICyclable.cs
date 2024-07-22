using System.Threading.Tasks;

namespace Turbo.Core.Game;

public interface ICyclable
{
    public Task Cycle();
}