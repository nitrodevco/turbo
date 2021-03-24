using System.Collections.Generic;
using Turbo.Core.Packets.Messages;
using Turbo.Database.Entities.Navigator;

namespace Turbo.Packets.Outgoing.Navigator
{
    public record NavigatorEventCategoriesMessage : IComposer
    {
        public List<NavigatorEventCategoryEntity> EventCategories { get; set; }
    }
}
