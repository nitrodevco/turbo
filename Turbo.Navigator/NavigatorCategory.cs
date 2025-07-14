using Turbo.Core.Database.Entities.Navigator;
using Turbo.Core.Game.Navigator;

namespace Turbo.Navigator;

public class NavigatorFlatCategory(NavigatorFlatCategoryEntity entity) : INavigatorCategory
{
    public int Id => entity.Id;
    public string Name => entity.Name;
    public bool Visible => entity.Visible;
    public bool Automatic => entity.Automatic;
    public string AutomaticCategoryKey => entity.AutomaticCategory;
    public string GlobalCategoryKey => entity.GlobalCategory;
    public bool StaffOnly => entity.StaffOnly;
    public int OrderNum => entity.OrderNum;
}