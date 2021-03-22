using Turbo.Core.Packets.Messages;

namespace Turbo.Core.Game.Rooms.Object.Data
{
    public interface IStuffData
    {
        public int Flags { get; set; }

        public void WriteToPacket(IServerPacket packet);
    }
}
