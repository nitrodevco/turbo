namespace Turbo.Packets.Shared.Navigator
{
    public record NavigatorSavedSearch
    {
        public int Id { get; init; }
        public string SearchCode { get; init; }
        public string Filter { get; init; }
        public string Localization { get; init; }
    }
}
