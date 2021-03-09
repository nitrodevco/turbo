namespace Turbo.Packets.Outgoing.Room.Engine
{
    public record FloorHeightMapMessage : IComposer
    {
        public bool IsNormalZoom { get; init; }
        public int WallHeight { get; init; }
        public string HeightMap { get; init; }
    }
}
