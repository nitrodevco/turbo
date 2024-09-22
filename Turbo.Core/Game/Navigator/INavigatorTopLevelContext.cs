namespace Turbo.Core.Game.Navigator;

public interface INavigatorTopLevelContext
{
    public int Id { get; }
    public string SearchCode { get; }
    public bool Visible { get; }
}