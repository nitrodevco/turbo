namespace Turbo.Packets.Incoming.Navigator
{
    public record NavigatorDeleteSavedSearchMessage : IMessageEvent
    {
        public int SearchID { get; init; }
    }
}
