namespace Turbo.Core.Game.Navigator;

public interface INavigatorTab
{
    public int Id { get; }
    public string SearchCode { get; }
    public ITopLevelContext TopLevelContext { get; }
}