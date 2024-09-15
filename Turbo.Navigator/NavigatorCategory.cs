using Turbo.Core.Game.Navigator;
using Turbo.Core.Database.Entities.Navigator;

namespace Turbo.Navigator;

public class NavigatorCategory(NavigatorCategoryEntity _categoryEntity) : INavigatorCategory
{
    public int Id => _categoryEntity.Id;
    public string Name => _categoryEntity.Name;
    public bool Visible => true;
    public bool Automatic => true;
    public string AutomaticCategoryKey => ""; // The client doesn't do anything with this value

    public string GlobalCategoryKey =>
        _categoryEntity
            .LocalizationName; // If this value is null / empty then the client will display the Name value, otherwise `navigator.flatcategory.global.${ GlobalCategoryKey }`

    public bool StaffOnly => false;
}