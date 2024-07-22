namespace Turbo.Core.Game.Navigator;

public interface INavigatorEventCategory
{
    public int Id { get; }
    public string Name { get; }
    public bool Enabled { get; }
}