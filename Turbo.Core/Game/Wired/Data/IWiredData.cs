using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Packets.Messages;

namespace Turbo.Core.Game.Wired.Data;

public interface IWiredData
{
    public int Id { get; }
    public int SpriteId { get; }
    public int WiredType { get; }
    public bool SelectionEnabled { get; }
    public int SelectionLimit { get; }
    public IList<int> SelectionIds { get; set; }
    public string StringParameter { get; set; }
    public IList<int> IntParameters { get; set; }
    public bool SetFromMessage(IMessageEvent update);
    public bool SetRoomObject(IRoomObjectFloor roomObject);
}