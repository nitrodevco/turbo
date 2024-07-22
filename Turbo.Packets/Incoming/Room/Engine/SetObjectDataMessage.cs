using System.Collections.Generic;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Engine;

public record SetObjectDataMessage : IMessageEvent
{
    public int ObjectId { get; init; }
    public Dictionary<string, string> Data { get; init; }
}