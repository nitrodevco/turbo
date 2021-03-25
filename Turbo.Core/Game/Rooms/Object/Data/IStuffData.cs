using Turbo.Core.Packets.Messages;

namespace Turbo.Core.Game.Rooms.Object.Data
{
    public interface IStuffData
    {
        public int Flags { get; set; }

        public void WriteToPacket(IServerPacket packet);
        public string GetLegacyString();
        public void SetState(string state);
        public int GetState();
        public bool IsUnique();
    }
}
