namespace Turbo.Core.Game.Navigator;

public interface INavigatorCategory
{
    public int Id { get; }
    public string Name { get; }
    public bool Visible { get; }
    public bool Automatic { get; }
    public bool StaffOnly { get; }
    public string AutomaticCategoryKey { get; }
    public string GlobalCategoryKey { get; }
    public int OrderNum { get; }
}