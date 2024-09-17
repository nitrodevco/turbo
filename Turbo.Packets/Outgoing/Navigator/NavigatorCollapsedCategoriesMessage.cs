using System.Collections.Generic;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Navigator;

public class NavigatorCollapsedCategoriesMessage : IComposer
{
    public List<string> CollapsedCategories { get; init; }
}