namespace Turbo.Rooms.Object.Data.Types
{
    public class EmptyStuffData : StuffDataBase
    {
        public string Data { get; set; }

        public override string GetLegacyString()
        {
            return Data == null ? "" : Data;
        }
    }
}
