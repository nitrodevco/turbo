using System.Collections.Generic;
using Turbo.Core.Game.Navigator;

namespace Turbo.Navigator;

public class NavigatorCollapsedCategories : INavigatorCollapsedCategories
{
    public List<string> CollapsedCategories { get; set; } = [];
}