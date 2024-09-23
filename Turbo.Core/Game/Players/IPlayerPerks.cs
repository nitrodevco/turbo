using System.Threading.Tasks;

namespace Turbo.Core.Game.Players;

public interface IPlayerPerks
{
    
    public Task<bool> HasPerkAsync(string perk);
    
}