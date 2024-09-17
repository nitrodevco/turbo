using System.Collections.Generic;

namespace Turbo.Core.Game.Navigator;

public interface INavigatorCollapsedCategories
{
    List<string> CollapsedCategories { get; set; }
}