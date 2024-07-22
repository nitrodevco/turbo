namespace Turbo.Core.Game.Navigator;

public interface INavigatorSavedSearch
{
    public int Id { get; }
    public string SearchCode { get; }
    public string Filter { get; }
    public string Localization { get; }
}