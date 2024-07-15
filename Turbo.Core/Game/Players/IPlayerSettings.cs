using System.Threading.Tasks;
using Turbo.Core.Utilities;

namespace Turbo.Core.Game.Players
{
    public interface IPlayerSettings : IComponent
    {
        int ChatStyle { get; set; }
        Task SaveSettings();
        Task LoadSettings();
    }
}