using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Navigator;
using Turbo.Database.Entities.Navigator;

namespace Turbo.Navigator
{
    public class NavigatorCategory(NavigatorCategoryEntity _categoryEntity) : INavigatorCategory
    {
        public int Id => _categoryEntity.Id;
        public string Name => _categoryEntity.Name;
        public bool Visible => true;
        public bool Automatic => true;
        public string? LocalizationName => _categoryEntity.LocalizationName;
    }
}
