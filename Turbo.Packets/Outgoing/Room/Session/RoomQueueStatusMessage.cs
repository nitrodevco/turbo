using System.Collections.Generic;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Session;

public record RoomQueueStatusMessage : IComposer
{
    //TODO: This is most likey wrong. Since you're here it means you're looking for the correct values.
    // The List's are most likely wrong. I'm not sure what the correct values are.
    // I got this from the OpenSource SWF and used chatgpt to generate the values.
    // So it might be wrong. idk.
    // With Much Love, Dippy
    // Hours Wasted: 0.1
    public int flatID { get; init; }
    public List<int> queueSetTargets { get; init; }
    public int activeTarget { get; init; }
    public List<string> queueTypes { get; init; }
}